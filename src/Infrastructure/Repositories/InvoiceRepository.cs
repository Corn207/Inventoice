using Application.Interfaces.Repositories;
using Domain.DTOs;
using Domain.DTOs.Invoices;
using Domain.Entities;
using Infrastructure.Repositories.Bases;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Infrastructure.Repositories;
public class InvoiceRepository(Database database)
	: SoftDeletableRepository<Invoice>(database), IInvoiceRepository
{
	private IMongoQueryable<Invoice> GetSearchQuery(
		string productNameOrBarcode,
		string clientNameOrPhonenumber,
		string authorName,
		InvoiceStatus status,
		TimeRange timeRange,
		OrderBy orderBy)
	{
		var query = Database.Collection<Invoice>().AsQueryable()
			.Where(x => x.DateDeleted == null);

		if (!string.IsNullOrWhiteSpace(productNameOrBarcode))
		{
			query = query.Where(x => x.ProductItems.Any(p =>
			p.Barcode.Contains(productNameOrBarcode, StringComparison.InvariantCultureIgnoreCase) ||
			p.Name.Contains(productNameOrBarcode, StringComparison.InvariantCultureIgnoreCase)));
		}

		if (!string.IsNullOrWhiteSpace(clientNameOrPhonenumber))
		{
			query = query.Where(x => x.Client != null &&
				(x.Client.Name.Contains(clientNameOrPhonenumber, StringComparison.InvariantCultureIgnoreCase) ||
				x.Client.Phonenumber.Contains(clientNameOrPhonenumber, StringComparison.InvariantCultureIgnoreCase)));
		}

		if (!string.IsNullOrWhiteSpace(authorName))
		{
			query = query.Where(x => x.Author.Name.Contains(authorName, StringComparison.InvariantCultureIgnoreCase));
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

		return query;
	}

	public async Task<List<Invoice>> SearchAsync(
		string productNameOrBarcode,
		string clientNameOrPhonenumber,
		string authorName,
		InvoiceStatus status,
		TimeRange timeRange,
		OrderBy orderBy,
		Pagination pagination)
	{
		var query = GetSearchQuery(
			productNameOrBarcode,
			clientNameOrPhonenumber,
			authorName,
			status,
			timeRange,
			orderBy);

		var result = await query
			.Skip((pagination.PageNumber - 1) * pagination.PageSize)
			.Take(pagination.PageSize)
			.ToListAsync();

		return result;
	}

	public async Task<uint> CountAsync(
		string productNameOrBarcode,
		string clientNameOrPhonenumber,
		string authorName,
		InvoiceStatus status,
		TimeRange timeRange,
		OrderBy orderBy)
	{
		var query = GetSearchQuery(
			productNameOrBarcode,
			clientNameOrPhonenumber,
			authorName,
			status,
			timeRange,
			orderBy);

		var result = await query.CountAsync();

		return Convert.ToUInt32(result);
	}
}
