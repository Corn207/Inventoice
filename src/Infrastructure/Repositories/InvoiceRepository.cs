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
	: SoftDeletableRepository<Invoice>(database), IInvoiceRepository
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
		var query = Database.Collection<Invoice>();
		var pipelineBuilder = new PipelineBuilder<Invoice>()
			.Match(Builders<Invoice>.Filter.Eq(nameof(Invoice.DateDeleted), BsonNull.Value))
			.MatchOr<InvoiceProductItem>(
				nameof(Invoice.ProductItems),
				(nameof(InvoiceProductItem.Name), productNameOrBarcode),
				(nameof(InvoiceProductItem.Barcode), productNameOrBarcode))
			.MatchOr(
				(nameof(Invoice.Client.Name), clientNameOrPhonenumber),
				(nameof(Invoice.Client.Phonenumber), clientNameOrPhonenumber))
			.MatchOr((nameof(Invoice.Author.Name), authorName));
		if (status != InvoiceStatus.All)
		{
			var filter = status switch
			{
				InvoiceStatus.Pending => Builders<Invoice>.Filter.Where(x => x.DatePaid == null && x.DateCancelled == null),
				InvoiceStatus.Paid => Builders<Invoice>.Filter.Where(x => x.DatePaid != null && x.DateCancelled == null),
				_ => Builders<Invoice>.Filter.Where(x => x.DateCancelled != null),
			};
			pipelineBuilder.Match(filter);
		}
		pipelineBuilder
			.Match(nameof(Invoice.DateCreated), timeRange)
			.Sort(nameof(Invoice.DateCreated), orderBy)
			.Paging(pagination);
		var pipeline = pipelineBuilder.Build();
		var result = await query.Aggregate(pipeline).ToListAsync();

		return result;
	}

	public async Task<uint> CountAsync(
		string productNameOrBarcode,
		string clientNameOrPhonenumber,
		string authorName,
		InvoiceStatus status,
		TimeRange timeRange)
	{
		var query = Database.Collection<Invoice>();
		var pipelineBuilder = new PipelineBuilder<Invoice>()
			.Match(Builders<Invoice>.Filter.Eq(nameof(Invoice.DateDeleted), BsonNull.Value))
			.MatchOr<InvoiceProductItem>(
				nameof(Invoice.ProductItems),
				(nameof(InvoiceProductItem.Name), productNameOrBarcode),
				(nameof(InvoiceProductItem.Barcode), productNameOrBarcode))
			.MatchOr(
				(nameof(Invoice.Client.Name), clientNameOrPhonenumber),
				(nameof(Invoice.Client.Phonenumber), clientNameOrPhonenumber))
			.MatchOr((nameof(Invoice.Author.Name), authorName));
		if (status != InvoiceStatus.All)
		{
			var filter = status switch
			{
				InvoiceStatus.Pending => Builders<Invoice>.Filter.Where(x => x.DatePaid == null && x.DateCancelled == null),
				InvoiceStatus.Paid => Builders<Invoice>.Filter.Where(x => x.DatePaid != null && x.DateCancelled == null),
				_ => Builders<Invoice>.Filter.Where(x => x.DateCancelled != null),
			};
			pipelineBuilder.Match(filter);
		}
		pipelineBuilder
			.Match(nameof(Invoice.DateCreated), timeRange);
		return await pipelineBuilder.BuildAndCount(query);
	}
}
