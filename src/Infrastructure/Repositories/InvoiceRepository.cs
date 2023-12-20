using Application.Interfaces.Repositories;
using Domain.DTOs;
using Domain.DTOs.Invoices;
using Domain.Entities;
using Infrastructure.Repositories.Bases;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Infrastructure.Repositories;
public class InvoiceRepository(Database database) : SoftDeletableRepository<Invoice>(database), IInvoiceRepository
{
	public async Task<List<Invoice>> SearchAsync(
		string productNameOrBarcode,
		string clientNameOrPhonenumber,
		string authorName,
		InvoiceStatus status,
		TimeRange timeRange,
		OrderBy orderBy,
		Pagination pagination)
	{
		var query = Database.Collection<Invoice>().AsQueryable();

		if (!string.IsNullOrWhiteSpace(productNameOrBarcode))
		{
			query = query.Where(x => x.ProductItems.Any(p => p.Barcode.Contains(productNameOrBarcode) || p.Name.Contains(productNameOrBarcode)));
		}

		if (!string.IsNullOrWhiteSpace(clientNameOrPhonenumber))
		{
			query = query.Where(x => x.Client != null && (x.Client.Name.Contains(clientNameOrPhonenumber) || x.Client.Phonenumber.Contains(clientNameOrPhonenumber)));
		}

		if (!string.IsNullOrWhiteSpace(authorName))
		{
			query = query.Where(x => x.Author.Name.Contains(authorName));
		}

		if (status != InvoiceStatus.All)
		{
			switch (status)
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

		if (orderBy == OrderBy.Ascending)
		{
			query = query.OrderBy(p => p.DateCreated);
		}
		else
		{
			query = query.OrderByDescending(p => p.DateCreated);
		}

		var products = await query
			.Skip((pagination.PageNumber - 1) * pagination.PageSize)
			.Take(pagination.PageSize)
			.ToListAsync();

		return products;
	}
}
