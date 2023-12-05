using Application.Interfaces.Repositories;
using Domain.DTOs.Invoices;
using Domain.Entities;
using Domain.Parameters;
using Infrastructure.Repositories.Bases;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Infrastructure.Repositories;
public class InvoiceRepository : SoftDeletableRepository<Invoice>, IInvoiceRepository
{
	public InvoiceRepository(Database database) : base(database)
	{
	}

	public async Task<List<InvoiceShort>> SearchAsync(
		InvoiceFilterParameters filter,
		PaginationParameters pagination,
		TimeRangeParameters timeRange,
		bool isDescending)
	{
		var query = Database.Collection<Invoice>().AsQueryable();

		if (!string.IsNullOrWhiteSpace(filter.ProductNameOrBarcode))
		{
			query = query.Where(x => x.ProductItems.Any(p => p.Barcode.Contains(filter.ProductNameOrBarcode) || p.Name.Contains(filter.ProductNameOrBarcode)));
		}

		if (!string.IsNullOrWhiteSpace(filter.ClientNameOrPhonenumber))
		{
			query = query.Where(x => x.Client != null && (x.Client.Name.Contains(filter.ClientNameOrPhonenumber) || x.Client.PhoneNumber.Contains(filter.ClientNameOrPhonenumber)));
		}

		if (!string.IsNullOrWhiteSpace(filter.AuthorName))
		{
			query = query.Where(x => x.Author.Name.Contains(filter.AuthorName));
		}

		if (filter.Status is not null)
		{
			switch (filter.Status)
			{
				case InvoiceStatus.Pending:
					query = query.Where(x => x.DatePaid == null && x.DateCancelled == null); break;
				case InvoiceStatus.Paid:
					query = query.Where(x => x.DatePaid != null && x.DateCancelled == null); break;
				case InvoiceStatus.Cancelled:
					query = query.Where(x => x.DateCancelled != null); break;
				default:
					break;
			}
		}

		if (timeRange.From != DateTime.MinValue)
		{
			query = query.Where(p => p.DateCreated >= timeRange.From);
		}

		if (timeRange.To != DateTime.MaxValue)
		{
			query = query.Where(p => p.DateCreated <= timeRange.To);
		}

		var projectedQuery = query.Select(x => new InvoiceShort()
		{
			Id = x.Id!,
			Status = x.DateCancelled != null ? InvoiceStatus.Cancelled : x.DatePaid != null ? InvoiceStatus.Paid : InvoiceStatus.Pending,
			ClientName = x.Client != null ? x.Client.Name : null,
			DateCreated = x.DateCreated,
			PaidAmount = x.PaidAmount,
			TotalProduct = (ushort)x.ProductItems.Count,
			FirstProductName = x.ProductItems[0].Name,
			FirstProductQuantity = x.ProductItems[0].Quantity
		});

		var products = await projectedQuery
			.Skip((pagination.PageNumber - 1) * pagination.PageSize)
			.Take(pagination.PageSize)
			.ToListAsync();

		return products;
	}
}
