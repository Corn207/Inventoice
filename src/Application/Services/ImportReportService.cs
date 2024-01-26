using Application.Exceptions;
using Application.Interfaces.Repositories;
using Domain.DTOs;
using Domain.DTOs.ImportReports;
using Domain.Entities;
using Domain.Mappers;

namespace Application.Services;
public class ImportReportService(
	IImportReportRepository importReportRepository,
	IProductRepository productRepository,
	IUserRepository userRepository)
{
	public async Task<IEnumerable<ImportReportShort>> SearchAsync(
		string? productNameOrBarcode,
		string? authorName,
		TimeRange timeRange,
		OrderBy orderBy,
		Pagination pagination)
	{
		var entities = await importReportRepository.SearchAsync(
			productNameOrBarcode,
			authorName,
			timeRange,
			orderBy,
			pagination);

		return entities.Select(ImportReportMapper.ToShort);
	}

	public async Task<uint> CountAllAsync()
	{
		var count = await importReportRepository.CountAllAsync();

		return count;
	}

	public async Task<ImportReport?> GetAsync(string id)
	{
		return await importReportRepository.GetAsync(id);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="authorUserId"></param>
	/// <param name="create"></param>
	/// <returns></returns>
	/// <exception cref="NotFoundException"></exception>
	public async Task<string> CreateAsync(string authorUserId, ImportReportCreate create)
	{
		var user = await userRepository.GetAsync(authorUserId)
			?? throw new NotFoundException("ImportReport.Author's UserId was not found.", authorUserId);
		var createIds = create.ProductItems.Select(x => x.Id).ToArray();
		var products = await productRepository.GetByIdsAsync(createIds);
		if (products.Count != create.ProductItems.Length)
		{
			var nonExistingIds = createIds.Except(products.Select(x => x.Id!));
			throw new NotFoundException("ImportReport.ProductItems' Ids were not found.", nonExistingIds.ToArray());
		}

		var changes = products
			.Join(
				create.ProductItems,
				product => product.Id,
				create => create.Id,
				(product, create) => new
				{
					create.Id,
					product.Name,
					product.Barcode,
					product.InStock,
					create.Quantity,
					create.Price
				})
			.ToArray();

		#region Creating entity
		var userInfo = UserMapper.ToInfo(user);
		var items = changes
			.Select(x => new ImportReportProductItem()
			{
				Id = x.Id,
				Name = x.Name,
				Barcode = x.Barcode,
				Quantity = x.Quantity,
				Price = x.Price
			})
			.ToList();
		var entity = new ImportReport()
		{
			Author = userInfo,
			ProductItems = items,
			DateCreated = DateTime.UtcNow
		};
		#endregion

		var tasks = changes
			.Select(x => productRepository.UpdateAsync(x.Id, (x => x.InStock, x.InStock + x.Quantity), (x => x.BuyingPrice, x.Price)))
			.Append(importReportRepository.CreateAsync(entity));
		await Task.WhenAll(tasks);

		return entity.Id!;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	/// <exception cref="NotFoundException"></exception>
	public async Task DeleteAsync(string id)
	{
		await importReportRepository.DeleteAsync(id);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	/// <exception cref="NotFoundException"></exception>
	/// <exception cref="OutOfStockException"></exception>
	public async Task CancelAsync(string id)
	{
		var report = await importReportRepository.GetAsync(id, x => x.DateCancelled == null)
			?? throw new NotFoundException("ImportReport was not found or cancelled.", id);
		var products = await productRepository.GetByIdsAsync(report.ProductItems.Select(x => x.Id));

		var changes = products
			.Join(
				report.ProductItems,
				product => product.Id,
				reportProduct => reportProduct.Id,
				(product, reportProduct) => new
				{
					reportProduct.Id,
					product.InStock,
					reportProduct.Quantity
				})
			.ToArray();

		var outOfStockProductIds = changes
			.Where(x => x.InStock < x.Quantity)
			.Select(x => x.Id)
			.ToArray();
		if (outOfStockProductIds.Length > 0)
		{
			throw new OutOfStockException(outOfStockProductIds);
		}

		var tasks = changes
			.Select(x => productRepository.UpdateAsync(x.Id, (x => x.InStock, x.InStock - x.Quantity)))
			.Append(importReportRepository.UpdateAsync(id, (x => x.DateCancelled, DateTime.UtcNow)));

		await Task.WhenAll(tasks);
	}
}
