using Application.Interfaces.Repositories;
using Domain.DTOs.AuditReports;
using Domain.Entities;
using Domain.Mappers;
using Domain.Parameters;

namespace Application.Services;
public class AuditReportService
{
	private readonly IAuditReportRepository _auditReportRepository;
	private readonly IProductRepository _productRepository;

	public AuditReportService(
		IAuditReportRepository auditReportRepository,
		IProductRepository productRepository)
	{
		_auditReportRepository = auditReportRepository;
		_productRepository = productRepository;
	}

	public async Task<IEnumerable<AuditReportShort>> SearchAsync(
		string? nameOrBarcode = null,
		ushort pageNumber = 1,
		ushort pageSize = 15,
		DateTime? startDate = null,
		DateTime? endDate = null,
		bool isDescending = false)
	{
		var pagination = new PaginationParameters(pageNumber, pageSize);
		var timeRange = new TimeRangeParameters(startDate ?? DateTime.MinValue, endDate ?? DateTime.MaxValue);
		var entities = await _auditReportRepository.SearchAsync(nameOrBarcode ?? string.Empty, pagination, timeRange, isDescending);

		return entities.Select(AuditReportMapper.ToShortForm);
	}

	public async Task<AuditReport?> GetAsync(string id)
	{
		return await _auditReportRepository.GetAsync(id);
	}

	public async Task<string> CreateAsync(AuditReportCreate create)
	{
		var products = await _productRepository
			.GetByIdsAsync(
				create.ProductItems.Select(x => x.ProductId),
				product => new
				{
					ProductId = product.Id!,
					Name = product.Name,
					Barcode = product.Barcode,
					StockCount = product.StockCount
				});

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
		var items = products
			.Join(
				create.ProductItems,
				product => product.ProductId,
				create => create.ProductId,
				(product, create) => new AuditReportProductItem()
				{
					ProductId = product.ProductId,
					Name = product.Name,
					Barcode = product.Barcode,
					OriginalQuantity = product.StockCount,
					AdjustedQuantity = create.AdjustedQuantity
				})
			.ToList();
		var entity = new AuditReport()
		{
			Author = userInfo,
			ProductItems = items,
			DateCreated = DateTime.Now
		};
		#endregion

		var tasks = create.ProductItems
			.Select(x => _productRepository.UpdateAsync(x.ProductId, x => x.StockCount, x.AdjustedQuantity))
			.Append(_auditReportRepository.CreateAsync(entity));
		await Task.WhenAll(tasks);

		return entity.Id!;
	}

	public async Task DeleteAsync(string id)
	{
		try
		{
			await _auditReportRepository.SoftDeleteAsync(id);
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
}
