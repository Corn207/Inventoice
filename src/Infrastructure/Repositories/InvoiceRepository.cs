using Application.Interfaces.Repositories;
using Domain.DTOs;
using Domain.DTOs.Invoices;
using Domain.Entities;
using Infrastructure.Repositories.Bases;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Repositories;
public class InvoiceRepository(Database database)
	: Repository<Invoice>(database), IInvoiceRepository
{
	public async Task<List<Invoice>> SearchAsync(
		string? productNameOrBarcode,
		string? clientNameOrPhonenumber,
		string? authorName,
		InvoiceStatus status,
		TimeRange timeRange,
		OrderBy orderBy,
		Pagination pagination)
	{
		var filters = new List<FilterDefinition<Invoice>>();
		filters.AddTextSearchCaseInsentitive(
			x => x.ProductItems,
			(x => x.Name, productNameOrBarcode),
			(x => x.Barcode, productNameOrBarcode));
		if (!string.IsNullOrWhiteSpace(clientNameOrPhonenumber))
		{
			var clientExistFilter = Builders<Invoice>.Filter.Ne(nameof(Invoice.Client), BsonNull.Value);
			var clientSearchFilter = Builders<Invoice>.Filter.Or(
				Utility.TextSearchCaseInsentitive<Invoice>(x => x.Client!.Name, clientNameOrPhonenumber),
				Utility.TextSearchCaseInsentitive<Invoice>(x => x.Client!.Phonenumber, clientNameOrPhonenumber));

			// Maybe clientExistFilter abundant cuz clientSearchFilter already check for null
			filters.Add(clientExistFilter);
			filters.Add(clientSearchFilter);
		}
		filters.AddTextSearchCaseInsentitive((x => x.Author.Name, authorName));
		if (status != InvoiceStatus.All)
		{
			var filter = status switch
			{
				InvoiceStatus.Pending => Builders<Invoice>.Filter.Where(x => x.DatePaid == null && x.DateCancelled == null),
				InvoiceStatus.Paid => Builders<Invoice>.Filter.Where(x => x.DatePaid != null && x.DateCancelled == null),
				_ => Builders<Invoice>.Filter.Where(x => x.DateCancelled != null),
			};
			filters.Add(filter);
		}
		filters.AddFilterTimeRange(x => x.DateCreated, timeRange);

		var result = await Database.Collection<Invoice>()
			.And(filters)
			.Sort(x => x.DateCreated, orderBy)
			.Paginate(pagination)
			.ToListAsync();

		return result;
	}
}
