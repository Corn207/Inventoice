using Application.Exceptions;
using Application.Interfaces.Repositories;
using Domain.DTOs;
using Domain.DTOs.ExportReports;
using Domain.Entities;
using Domain.Mappers;

namespace Application.Services;
public class ExportReportService(
	IExportReportRepository exportReportRepository,
	IProductRepository productRepository)
{
	public async Task<IEnumerable<ExportReportShort>> SearchAsync(
		string nameOrBarcode,
		ushort pageNumber,
		ushort pageSize,
		DateTime startDate,
		DateTime endDate,
		OrderBy orderBy)
	{
		var pagination = new Pagination(pageNumber, pageSize);
		var timeRange = new TimeRange(startDate, endDate);
		var entities = await exportReportRepository.SearchAsync(nameOrBarcode, pagination, timeRange, orderBy);

		return entities.Select(ExportReportMapper.ToShortForm);
	}

	public async Task<ExportReport?> GetAsync(string id)
	{
		return await exportReportRepository.GetAsync(id);
	}

	public async Task<string> CreateAsync(ExportReportCreate create)
	{
		var createIds = create.ProductItems.Select(x => x.ProductId).ToArray();
		var products = await productRepository
			.GetByIdsAsync(
				createIds,
				x => new
				{
					ProductId = x.Id!,
					Name = x.Name,
					Barcode = x.Barcode,
					StockCount = x.StockCount
				});

		if (products.Count != create.ProductItems.Length)
		{
			var nonExistingIds = createIds.Except(products.Select(x => x.ProductId));
			throw new InvalidIdException(nonExistingIds.ToArray());
		}

		var changes = products
			.Join(
				create.ProductItems,
				product => product.ProductId,
				create => create.ProductId,
				(product, create) => new
				{
					ProductId = product.ProductId,
					Name = product.Name,
					Barcode = product.Barcode,
					StockCount = product.StockCount,
					Quantity = create.Quantity
				})
			.ToArray();

		var outOfStockProductIds = changes
			.Where(x => x.StockCount < x.Quantity)
			.Select(x => x.ProductId)
			.ToArray();
		if (outOfStockProductIds.Length > 0)
		{
			throw new OutOfStockException(outOfStockProductIds);
		}

		#region Creating entity
		var user = new User()
		{
			Id = "testId",
			Name = "Test User Name"
		};
		var userInfo = new UserInfo()
		{
			UserId = user.Id,
			Name = user.Name,
		};
		var items = changes
			.Select(x => new ExportReportProductItem()
			{
				ProductId = x.ProductId,
				Name = x.Name,
				Barcode = x.Barcode,
				Quantity = x.Quantity
			})
			.ToList();
		var entity = new ExportReport()
		{
			Author = userInfo,
			ProductItems = items,
			DateCreated = DateTime.Now
		};
		#endregion

		var tasks = changes
			.Select(x => productRepository.UpdateAsync(x.ProductId, x => x.StockCount, x.StockCount - x.Quantity))
			.Append(exportReportRepository.CreateAsync(entity));
		await Task.WhenAll(tasks);

		return entity.Id!;
	}

	public async Task DeleteAsync(string id)
	{
		try
		{
			await exportReportRepository.SoftDeleteAsync(id);
		}
		catch (KeyNotFoundException)
		{
			throw;
		}
		catch (Exception)
		{
			throw;
		}
	}

	public async Task CancelAsync(string id)
	{
		var reportProducts = await exportReportRepository.GetAsync(
			id,
			x => x.ProductItems.Select(i => new
			{
				ProductId = i.ProductId,
				Quantity = i.Quantity
			}).ToArray(),
			x => x.DateCancelled == null) ?? throw new KeyNotFoundException("Id was not found or cancelled or deleted.");

		var products = await productRepository.GetByIdsAsync(
			reportProducts.Select(x => x.ProductId),
			x => new
			{
				Id = x.Id!,
				StockCount = x.StockCount
			});

		var tasks = products
			.Join(
				reportProducts,
				product => product.Id,
				report => report.ProductId,
				(product, report) => productRepository.UpdateAsync(product.Id, x => x.StockCount, product.StockCount + report.Quantity))
			.Append(exportReportRepository.UpdateAsync(id, x => x.DateCancelled, DateTime.Now));

		await Task.WhenAll(tasks);
	}
}
