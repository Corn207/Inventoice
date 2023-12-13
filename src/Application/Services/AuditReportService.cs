using Application.Interfaces.Repositories;
using Domain.DTOs;
using Domain.DTOs.AuditReports;
using Domain.Entities;
using Domain.Mappers;

namespace Application.Services;
public class AuditReportService(
	IAuditReportRepository auditReportRepository,
	IProductRepository productRepository)
{
	public async Task<IEnumerable<AuditReportShort>> SearchAsync(
		string nameOrBarcode,
		ushort pageNumber,
		ushort pageSize,
		DateTime startDate,
		DateTime endDate,
		OrderBy orderBy)
	{
		var pagination = new Pagination(pageNumber, pageSize);
		var timeRange = new TimeRange(startDate, endDate);
		var entities = await auditReportRepository.SearchAsync(nameOrBarcode, pagination, timeRange, orderBy);

		return entities.Select(AuditReportMapper.ToShortForm);
	}

	public async Task<AuditReport?> GetAsync(string id)
	{
		return await auditReportRepository.GetAsync(id);
	}

	public async Task<string> CreateAsync(AuditReportCreate create)
	{
		var products = await productRepository
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
			.Select(x => productRepository.UpdateAsync(x.ProductId, x => x.StockCount, x.AdjustedQuantity))
			.Append(auditReportRepository.CreateAsync(entity));
		await Task.WhenAll(tasks);

		return entity.Id!;
	}

	public async Task DeleteAsync(string id)
	{
		try
		{
			await auditReportRepository.SoftDeleteAsync(id);
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
