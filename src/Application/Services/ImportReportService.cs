using Application.Exceptions;
using Application.Interfaces.Repositories;
using Domain.DTOs.ImportReports;
using Domain.Entities;
using Domain.Mappers;
using Domain.Parameters;

namespace Application.Services;
public class ImportReportService
{
	private readonly IImportReportRepository _importReportRepository;
	private readonly IProductRepository _productRepository;

	public ImportReportService(
		IImportReportRepository ImportReportRepository,
		IProductRepository productRepository)
	{
		_importReportRepository = ImportReportRepository;
		_productRepository = productRepository;
	}

	public async Task<IEnumerable<ImportReportShort>> SearchAsync(
		string? nameOrBarcode = null,
		ushort pageNumber = 1,
		ushort pageSize = 15,
		DateTime? startDate = null,
		DateTime? endDate = null,
		bool isDescending = false)
	{
		var pagination = new PaginationParameters(pageNumber, pageSize);
		var timeRange = new TimeRangeParameters(startDate ?? DateTime.MinValue, endDate ?? DateTime.MaxValue);
		var entities = await _importReportRepository.SearchAsync(nameOrBarcode ?? string.Empty, pagination, timeRange, isDescending);

		return entities.Select(ImportReportMapper.ToShortForm);
	}

	public async Task<ImportReport?> GetAsync(string id)
	{
		return await _importReportRepository.GetAsync(id);
	}

	public async Task<string> CreateAsync(ImportReportCreate create)
	{
		var createIds = create.ProductItems.Select(x => x.ProductId).ToArray();
		var products = await _productRepository
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
			.Select(x => _productRepository.UpdateAsync(x.ProductId, x => x.StockCount, x.StockCount + x.Quantity, x => x.LastImportedPrice, x.UnitPrice))
			.Append(_importReportRepository.CreateAsync(entity));
		await Task.WhenAll(tasks);

		return entity.Id!;
	}

	public async Task DeleteAsync(string id)
	{
		try
		{
			await _importReportRepository.SoftDeleteAsync(id);
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
		var reportProducts = await _importReportRepository.GetAsync(
			id,
			x => x.ProductItems.Select(i => new
			{
				ProductId = i.ProductId,
				Quantity = i.Quantity,
			}).ToArray(),
			x => x.DateCancelled == null) ?? throw new KeyNotFoundException("Id was not found or cancelled or deleted.");

		var products = await _productRepository.GetByIdsAsync(
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
			.Select(x => _productRepository.UpdateAsync(x.ProductId, x => x.StockCount, x.StockCount - x.Quantity))
			.Append(_importReportRepository.UpdateAsync(id, x => x.DateCancelled, DateTime.Now));

		await Task.WhenAll(tasks);
	}
}
