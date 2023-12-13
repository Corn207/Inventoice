using Application.Exceptions;
using Application.Interfaces.Repositories;
using Domain.DTOs;
using Domain.DTOs.ImportReports;
using Domain.Entities;
using Domain.Mappers;

namespace Application.Services;
public class ImportReportService(
	IImportReportRepository ImportReportRepository,
	IProductRepository productRepository)
{
	public async Task<IEnumerable<ImportReportShort>> SearchAsync(
		string nameOrBarcode,
		ushort pageNumber,
		ushort pageSize,
		DateTime startDate,
		DateTime endDate,
		OrderBy orderBy)
	{
		var pagination = new Pagination(pageNumber, pageSize);
		var timeRange = new TimeRange(startDate, endDate);
		var entities = await ImportReportRepository.SearchAsync(nameOrBarcode, pagination, timeRange, orderBy);

		return entities.Select(ImportReportMapper.ToShortForm);
	}

	public async Task<ImportReport?> GetAsync(string id)
	{
		return await ImportReportRepository.GetAsync(id);
	}

	public async Task<string> CreateAsync(ImportReportCreate create)
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
					Quantity = create.Quantity,
					UnitPrice = create.UnitPrice
				})
			.ToArray();

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
			.Select(x => new ImportReportProductItem()
			{
				ProductId = x.ProductId,
				Name = x.Name,
				Barcode = x.Barcode,
				Quantity = x.Quantity,
				UnitPrice = x.UnitPrice
			})
			.ToList();
		var entity = new ImportReport()
		{
			Author = userInfo,
			ProductItems = items,
			DateCreated = DateTime.Now
		};
		#endregion

		var tasks = changes
			.Select(x => productRepository.UpdateAsync(x.ProductId, x => x.StockCount, x.StockCount + x.Quantity, x => x.LastImportedPrice, x.UnitPrice))
			.Append(ImportReportRepository.CreateAsync(entity));
		await Task.WhenAll(tasks);

		return entity.Id!;
	}

	public async Task DeleteAsync(string id)
	{
		try
		{
			await ImportReportRepository.SoftDeleteAsync(id);
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
		var reportProducts = await ImportReportRepository.GetAsync(
			id,
			x => x.ProductItems.Select(i => new
			{
				ProductId = i.ProductId,
				Quantity = i.Quantity,
			}).ToArray(),
			x => x.DateCancelled == null) ?? throw new KeyNotFoundException("Id was not found or cancelled or deleted.");

		var products = await productRepository.GetByIdsAsync(
			reportProducts.Select(x => x.ProductId),
			x => new
			{
				ProductId = x.Id!,
				StockCount = x.StockCount,
			});

		var changes = products
			.Join(
				reportProducts,
				product => product.ProductId,
				reportProduct => reportProduct.ProductId,
				(product, reportProduct) => new
				{
					ProductId = product.ProductId,
					StockCount = product.StockCount,
					Quantity = reportProduct.Quantity
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

		var tasks = changes
			.Select(x => productRepository.UpdateAsync(x.ProductId, x => x.StockCount, x.StockCount - x.Quantity))
			.Append(ImportReportRepository.UpdateAsync(id, x => x.DateCancelled, DateTime.Now));

		await Task.WhenAll(tasks);
	}
}
