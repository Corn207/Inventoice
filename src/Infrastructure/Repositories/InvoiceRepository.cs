using Application.Interfaces.Repositories;
using Domain.DTOs;
using Domain.DTOs.Invoices;
using Domain.Entities;
using Infrastructure.Repositories.Bases;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Infrastructure.Repositories;
public class InvoiceRepository(Database database)
	: Repository<Invoice>(database), IInvoiceRepository
{
	public async Task<PartialEnumerable<Invoice>> SearchAsync(
		string? productNameOrBarcode,
		string? clientNameOrPhonenumber,
		string? authorName,
		InvoiceStatus status,
		TimeRange timeRange,
		OrderBy orderBy,
		Pagination pagination)
	{
		var query = Database.Collection<Invoice>();
		PipelineDefinition<Invoice, Invoice> pipeline = new EmptyPipelineDefinition<Invoice>();

		var filters = new List<FilterDefinition<Invoice>>();

		filters.AddFilterArrayContainsAnyRegex(
			x => x.ProductItems,
			[
				(x => x.Name, productNameOrBarcode),
				(x => x.Barcode, productNameOrBarcode)
			]);

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

		filters.AddFilter(x => x.Author.Name, authorName);

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

		filters.AddFilterTimeRange(x => x.DateCreated, timeRange.From, timeRange.To);
		var matchStage = Utility.BuildStageMatchAnd(filters);
		if (matchStage is not null)
		{
			pipeline = pipeline.AppendStage(matchStage);
		};

		var sortStage = Utility.BuildStageSort<Invoice>(x => x.DateCreated, orderBy);
		pipeline = pipeline.AppendStage(sortStage);

		var groupStage = Utility.BuildStageGroupAndPage<Invoice>(pagination);
		var finalPipeline = pipeline.AppendStage(groupStage);

		var result = await query.Aggregate(finalPipeline).FirstAsync();

		return result;
	}
}
