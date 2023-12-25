using Application.Interfaces.Repositories;
using Domain.DTOs;
using Domain.Entities;
using Infrastructure.Repositories.Bases;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Infrastructure.Repositories;
public class ExportReportRepository(Database database)
	: SoftDeletableRepository<ExportReport>(database), IExportReportRepository
{
	public async Task<List<ExportReport>> SearchAsync(
		string productNameOrBarcode,
		string authorName,
		TimeRange timeRange,
		OrderBy orderBy,
		Pagination pagination)
	{
		var query = Database.Collection<ExportReport>();
		var pipelineBuilder = new PipelineBuilder<ExportReport>()
			.Match(Builders<ExportReport>.Filter.Eq(nameof(ExportReport.DateDeleted), BsonNull.Value))
			.MatchOr<ExportReportProductItem>(
				nameof(ExportReport.ProductItems),
				(nameof(ExportReportProductItem.Name), productNameOrBarcode),
				(nameof(ExportReportProductItem.Barcode), productNameOrBarcode))
			.MatchOr((nameof(ExportReport.Author.Name), authorName))
			.Match(nameof(ExportReport.DateCreated), timeRange)
			.Sort(nameof(ExportReport.DateCreated), orderBy)
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
		var query = Database.Collection<ExportReport>();
		var pipelineBuilder = new PipelineBuilder<ExportReport>()
			.Match(Builders<ExportReport>.Filter.Eq(nameof(ExportReport.DateDeleted), BsonNull.Value))
			.MatchOr<ExportReportProductItem>(
				nameof(ExportReport.ProductItems),
				(nameof(ExportReportProductItem.Name), productNameOrBarcode),
				(nameof(ExportReportProductItem.Barcode), productNameOrBarcode))
			.MatchOr((nameof(ExportReport.Author.Name), authorName))
			.Match(nameof(ExportReport.DateCreated), timeRange);
		var pipeline = pipelineBuilder.BuildCount();
		var result = await query.Aggregate(pipeline).FirstOrDefaultAsync();

		return Convert.ToUInt32(result.Count);
	}
}
