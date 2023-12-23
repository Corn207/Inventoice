using Application.Exceptions;
using Application.Interfaces.Repositories;
using Domain.DTOs;
using Domain.DTOs.AuditReports;
using Domain.Entities;
using Domain.Mappers;

namespace Application.Services;
public class AuditReportService(
	IAuditReportRepository auditReportRepository,
	IProductRepository productRepository,
	IUserRepository userRepository)
{
	public async Task<IEnumerable<AuditReportShort>> SearchAsync(
		string productNameOrBarcode,
		string authorName,
		TimeRange timeRange,
		OrderBy orderBy,
		Pagination pagination)
	{
		var entities = await auditReportRepository.SearchAsync(productNameOrBarcode, authorName, timeRange, orderBy, pagination);

		return entities.Select(AuditReportMapper.ToShort);
	}

	public async Task<uint> CountAsync(string productNameOrBarcode,
		string authorName,
		TimeRange timeRange,
		OrderBy orderBy)
	{
		var count = await auditReportRepository.CountAsync(productNameOrBarcode, authorName, timeRange, orderBy);

		return count;
	}

	public async Task<AuditReport?> GetAsync(string id)
	{
		return await auditReportRepository.GetAsync(id);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="create"></param>
	/// <returns></returns>
	/// <exception cref="InvalidIdException"></exception>
	/// <exception cref="UnknownException"></exception>
	public async Task<string> CreateAsync(AuditReportCreate create)
	{
		var user = await userRepository.GetAsync(create.AuthorUserId) ?? throw new InvalidIdException("UserId was not found.", [create.AuthorUserId]);
		var products = await productRepository.GetByIdsAsync(create.ProductItems.Select(x => x.Id));

		#region Creating entity
		var userInfo = UserMapper.ToInfo(user);
		var items = products
			.Join(
				create.ProductItems,
				product => product.Id,
				create => create.Id,
				(product, create) => new AuditReportProductItem()
				{
					Id = product.Id!,
					Name = product.Name,
					Barcode = product.Barcode,
					OriginalQuantity = product.InStock,
					AdjustedQuantity = create.Quantity
				})
			.ToList();
		var entity = new AuditReport()
		{
			Author = userInfo,
			ProductItems = items,
			DateCreated = DateTime.UtcNow
		};
		#endregion

		var tasks = create.ProductItems
			.Select(x => productRepository.UpdateAsync(x.Id, x => x.InStock, x.Quantity))
			.Append(auditReportRepository.CreateAsync(entity));
		await Task.WhenAll(tasks);

		return entity.Id!;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <exception cref="InvalidIdException"></exception>
	/// <exception cref="UnknownException"></exception>
	/// <returns></returns>
	public async Task DeleteAsync(string id)
	{
		await auditReportRepository.SoftDeleteAsync(id);
	}
}
