using Application.Interfaces.Repositories;
using Domain.DTOs;
using Domain.Entities;
using Infrastructure.Repositories.Bases;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Infrastructure.Repositories;
public class ImportReportRepository(Database database)
	: SoftDeletableRepository<ImportReport>(database), IImportReportRepository
{
	public async Task<List<ImportReport>> SearchAsync(
		string productNameOrBarcode,
		string authorName,
		TimeRange timeRange,
		OrderBy orderBy,
		Pagination pagination)
	{
		var query = Database.Collection<ImportReport>();
		var pipelineBuilder = new PipelineBuilder<ImportReport>()
			.Match(Builders<ImportReport>.Filter.Eq(nameof(ImportReport.DateDeleted), BsonNull.Value))
			.MatchOr<ImportReportProductItem>(
				nameof(ImportReport.ProductItems),
				(nameof(ImportReportProductItem.Name), productNameOrBarcode),
				(nameof(ImportReportProductItem.Barcode), productNameOrBarcode))
			.MatchOr((nameof(ImportReport.Author.Name), authorName))
			.Match(nameof(ImportReport.DateCreated), timeRange)
			.Sort(nameof(ImportReport.DateCreated), orderBy)
			.Paging(pagination);
		var pipeline = pipelineBuilder.Build();
		var result = await query.Aggregate(pipeline).ToListAsync();

		return result;
	}

	public async Task<uint> CountAsync(
		string productNameOrBarcode,
		string authorName,
		TimeRange timeRange)
	{
		var query = Database.Collection<ImportReport>();
		var pipelineBuilder = new PipelineBuilder<ImportReport>()
			.Match(Builders<ImportReport>.Filter.Eq(nameof(ImportReport.DateDeleted), BsonNull.Value))
			.MatchOr<ImportReportProductItem>(
				nameof(ImportReport.ProductItems),
				(nameof(ImportReportProductItem.Name), productNameOrBarcode),
				(nameof(ImportReportProductItem.Barcode), productNameOrBarcode))
			.MatchOr((nameof(ImportReport.Author.Name), authorName))
			.Match(nameof(ImportReport.DateCreated), timeRange);
		return await pipelineBuilder.BuildAndCount(query);
	}
}
