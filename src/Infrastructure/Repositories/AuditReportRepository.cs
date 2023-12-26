using Application.Interfaces.Repositories;
using Domain.DTOs;
using Domain.Entities;
using Infrastructure.Repositories.Bases;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Infrastructure.Repositories;
public class AuditReportRepository(Database database)
	: SoftDeletableRepository<AuditReport>(database), IAuditReportRepository
{
	public async Task<List<AuditReport>> SearchAsync(
		string productNameOrBarcode,
		string authorName,
		TimeRange timeRange,
		OrderBy orderBy,
		Pagination pagination)
	{
		var query = Database.Collection<AuditReport>();
		var pipelineBuilder = new PipelineBuilder<AuditReport>()
			.Match(Builders<AuditReport>.Filter.Eq(nameof(AuditReport.DateDeleted), BsonNull.Value))
			.MatchOr<AuditReportProductItem>(
				nameof(AuditReport.ProductItems),
				(nameof(AuditReportProductItem.Name), productNameOrBarcode),
				(nameof(AuditReportProductItem.Barcode), productNameOrBarcode))
			.MatchOr((nameof(AuditReport.Author.Name), authorName))
			.Match(nameof(AuditReport.DateCreated), timeRange)
			.Sort(nameof(AuditReport.DateCreated), orderBy)
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
		var query = Database.Collection<AuditReport>();
		var pipelineBuilder = new PipelineBuilder<AuditReport>()
			.Match(Builders<AuditReport>.Filter.Eq(nameof(AuditReport.DateDeleted), BsonNull.Value))
			.MatchOr<AuditReportProductItem>(
				nameof(AuditReport.ProductItems),
				(nameof(AuditReportProductItem.Name), productNameOrBarcode),
				(nameof(AuditReportProductItem.Barcode), productNameOrBarcode))
			.MatchOr((nameof(AuditReport.Author.Name), authorName))
			.Match(nameof(AuditReport.DateCreated), timeRange);
		return await pipelineBuilder.BuildAndCount(query);
	}
}
