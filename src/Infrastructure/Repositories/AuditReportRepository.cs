using Application.Interfaces.Repositories;
using Domain.DTOs;
using Domain.Entities;
using Infrastructure.Repositories.Bases;
using MongoDB.Driver;

namespace Infrastructure.Repositories;
public class AuditReportRepository(Database database)
	: Repository<AuditReport>(database), IAuditReportRepository
{
	public async Task<PartialEnumerable<AuditReport>> SearchAsync(
		string? productNameOrBarcode,
		string? authorName,
		TimeRange timeRange,
		OrderBy orderBy,
		Pagination pagination)
	{
		var query = Database.Collection<AuditReport>();
		PipelineDefinition<AuditReport, AuditReport> pipeline = new EmptyPipelineDefinition<AuditReport>();

		var filters = new List<FilterDefinition<AuditReport>>();
		filters.AddFilterArrayContainsAnyRegex(
			x => x.ProductItems,
			[
				(x => x.Name, productNameOrBarcode),
				(x => x.Barcode, productNameOrBarcode)
			]);
		filters.AddFilter(x => x.Author.Name, authorName);
		filters.AddFilterTimeRange(x => x.DateCreated, timeRange.From, timeRange.To);
		var matchStage = Utility.BuildStageMatchAnd(filters);
		if (matchStage is not null)
		{
			pipeline = pipeline.AppendStage(matchStage);
		};

		var sortStage = Utility.BuildStageSort<AuditReport>(x => x.DateCreated, orderBy);
		pipeline = pipeline.AppendStage(sortStage);

		var groupStage = Utility.BuildStageGroupAndPage<AuditReport>(pagination);
		var finalPipeline = pipeline.AppendStage(groupStage);

		var result = await query.Aggregate(finalPipeline).FirstOrDefaultAsync();
		result ??= new PartialEnumerable<AuditReport>([], 0);

		return result;
	}
}
