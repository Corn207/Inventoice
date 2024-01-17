using Application.Exceptions;
using Application.Interfaces.Repositories;
using Domain.DTOs;
using Domain.DTOs.Invoices;
using Domain.Entities;
using Domain.Mappers;

namespace Application.Services;
public class InvoiceService(
	IInvoiceRepository invoiceRepository,
	IClientRepository clientRepository,
	IProductRepository productRepository,
	IExportReportRepository exportReportRepository,
	IUserRepository userRepository)
{
	public async Task<IEnumerable<InvoiceShort>> SearchAsync(
		string? productNameOrBarcode,
		string? clientNameOrPhonenumber,
		string? authorName,
		InvoiceStatus status,
		TimeRange timeRange,
		OrderBy orderBy,
		Pagination pagination)
	{
		var entities = await invoiceRepository.SearchAsync(
			productNameOrBarcode,
			clientNameOrPhonenumber,
			authorName,
			status,
			timeRange,
			orderBy,
			pagination);

		return entities.Select(InvoiceMapper.ToShort);
	}

	public async Task<uint> CountAllAsync()
	{
		var count = await invoiceRepository.CountAllAsync();

		return count;
	}

	public async Task<Invoice?> GetAsync(string id)
	{
		return await invoiceRepository.GetAsync(id);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="create"></param>
	/// <returns></returns>
	/// <exception cref="NotFoundException"></exception>
	/// <exception cref="OutOfStockException"></exception>
	public async Task<string> CreateAsync(string authorUserId, InvoiceCreate create)
	{
		var user = await userRepository.GetAsync(authorUserId)
			?? throw new NotFoundException("Invoice.Author's UserId was not found.", authorUserId);

		ClientInfo? clientInfo = null;
		if (create.ClientId is not null)
		{
			var client = await clientRepository.GetAsync(create.ClientId)
				?? throw new NotFoundException("Invoice.Client's Id was not found.", create.ClientId);
			clientInfo = ClientMapper.ToInfo(client);
		}

		var createIds = create.ProductItems.Select(x => x.Id).ToArray();
		var products = await productRepository.GetByIdsAsync(createIds);
		if (products.Count != create.ProductItems.Length)
		{
			var nonExistingIds = createIds.Except(products.Select(x => x.Id!));
			throw new NotFoundException("Invoice.ProductItems' Ids were not found.", nonExistingIds.ToArray());
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
					product.SellingPrice,
					create.Quantity
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

		var userInfo = UserMapper.ToInfo(user);
		var timeNow = DateTime.UtcNow;


		#region Creating export report
		var exportReportProductItems = changes
			.Select(x => new ExportReportProductItem()
			{
				Id = x.Id,
				Name = x.Name,
				Barcode = x.Barcode,
				Quantity = x.Quantity
			})
			.ToList();
		var exportReportEntity = new ExportReport()
		{
			Author = userInfo,
			ProductItems = exportReportProductItems,
			DateCreated = timeNow,
		};

		await exportReportRepository.CreateAsync(exportReportEntity);
		#endregion

		#region Creating invoice
		var invoiceProductItems = changes
			.Select(x => new InvoiceProductItem()
			{
				Id = x.Id,
				Name = x.Name,
				Barcode = x.Barcode,
				Price = x.SellingPrice,
				Quantity = x.Quantity
			})
			.ToList();
		var invoiceEntity = new Invoice()
		{
			Author = userInfo,
			Client = clientInfo,
			ExportReportId = exportReportEntity.Id!,
			ProductItems = invoiceProductItems,
			GrandTotal = create.GrandTotal,
			DateCreated = timeNow,
			DatePaid = create.IsPaid ? timeNow : null,
		};

		await invoiceRepository.CreateAsync(invoiceEntity);
		#endregion

		var tasks = changes
			.Select(change => productRepository.UpdateAsync(change.Id, (x => x.InStock, change.InStock - change.Quantity)))
			.Append(exportReportRepository.UpdateAsync(exportReportEntity.Id!, (x => x.InvoiceId, invoiceEntity.Id!)));
		await Task.WhenAll(tasks);

		return invoiceEntity.Id!;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	/// <exception cref="NotFoundException"></exception>
	public async Task DeleteAsync(string id)
	{
		await invoiceRepository.DeleteAsync(id);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	/// <exception cref="NotFoundException"></exception>
	public async Task CancelAsync(string id)
	{
		var invoice = await invoiceRepository.GetAsync(id, x => x.DateCancelled == null)
			?? throw new NotFoundException("Invoice was not found or cancelled.", id);

		var products = await productRepository.GetByIdsAsync(invoice.ProductItems.Select(x => x.Id));

		var timeCancelled = DateTime.UtcNow;
		var tasks = products
			.Join(
				invoice.ProductItems,
				product => product.Id,
				invoiceProductItem => invoiceProductItem.Id,
				(product, invoiceProductItem) => productRepository.UpdateAsync(invoiceProductItem.Id, (x => x.InStock, product.InStock + invoiceProductItem.Quantity)))
			.Append(invoiceRepository.UpdateAsync(id, (x => x.DateCancelled, timeCancelled)))
			.Append(exportReportRepository.UpdateAsync(invoice.ExportReportId, (x => x.DateCancelled, timeCancelled)));
		await Task.WhenAll(tasks);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	/// <exception cref="NotFoundException"></exception>
	public async Task PayAsync(string id)
	{
		await invoiceRepository.UpdateAsync(
			[
				(x => x.Id == id),
				(x => x.DatePaid == null),
				(x => x.DateCancelled == null)
			],
			(x => x.DatePaid, DateTime.UtcNow));
	}
}
