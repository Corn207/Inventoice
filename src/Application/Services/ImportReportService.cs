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
		string productNameOrBarcode,
		string authorName,
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

	public async Task<uint> CountAsync(
		string productNameOrBarcode,
		string authorName,
		TimeRange timeRange)
	{
		var count = await importReportRepository.CountAsync(
			productNameOrBarcode,
			authorName,
			timeRange);

		return count;
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
	/// <param name="create"></param>
	/// <returns></returns>
	/// <exception cref="InvalidIdException"></exception>
	/// <exception cref="UnknownException"></exception>
	public async Task<string> CreateAsync(ImportReportCreate create)
	{
		var user = await userRepository.GetAsync(create.AuthorUserId)
			?? throw new InvalidIdException("UserId was not found.", [create.AuthorUserId]);
		var createIds = create.ProductItems.Select(x => x.Id).ToArray();
		var products = await productRepository.GetByIdsAsync(createIds);
		if (products.Count != create.ProductItems.Length)
		{
			var nonExistingIds = createIds.Except(products.Select(x => x.Id!));
			throw new InvalidIdException("ProductIds were not found.", nonExistingIds.ToArray());
		}

		var changes = products
			.Join(
				create.ProductItems,
				product => product.Id,
				create => create.Id,
				(product, create) => new
				{
					Id = create.Id,
					Name = product.Name,
					Barcode = product.Barcode,
					InStock = product.InStock,
					Quantity = create.Quantity,
					Price = create.Price
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
			.Select(x => productRepository.UpdateAsync(x.Id, x => x.InStock, x.InStock + x.Quantity, x => x.BuyingPrice, x.Price))
			.Append(importReportRepository.CreateAsync(entity));
		await Task.WhenAll(tasks);

		return entity.Id!;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	/// <exception cref="InvalidIdException"></exception>
	/// <exception cref="UnknownException"></exception>
	public async Task DeleteAsync(string id)
	{
		await importReportRepository.SoftDeleteAsync(id);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	/// <exception cref="InvalidIdException"></exception>
	/// <exception cref="OutOfStockException"></exception>
	/// <exception cref="UnknownException"></exception>
	public async Task CancelAsync(string id)
	{
		var report = await importReportRepository.GetAsync(id, x => x.DateCancelled == null)
			?? throw new InvalidIdException("Id was not found or cancelled or deleted.", [id]);
		var products = await productRepository.GetByIdsAsync(report.ProductItems.Select(x => x.Id));

		var changes = products
			.Join(
				report.ProductItems,
				product => product.Id,
				reportProduct => reportProduct.Id,
				(product, reportProduct) => new
				{
					Id = reportProduct.Id,
					InStock = product.InStock,
					Quantity = reportProduct.Quantity
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
			.Select(x => productRepository.UpdateAsync(x.Id, x => x.InStock, x.InStock - x.Quantity))
			.Append(importReportRepository.UpdateAsync(id, x => x.DateCancelled, DateTime.UtcNow));

		await Task.WhenAll(tasks);
	}
}
