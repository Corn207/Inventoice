using Application.Interfaces.Repositories;
using Domain.DTOs;
using Domain.Entities;
using Infrastructure.Repositories.Bases;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Infrastructure.Repositories;
public class ImportReportRepository(Database database)
	: Repository<ImportReport>(database), IImportReportRepository
{
	public async Task<PartialEnumerable<ImportReport>> SearchAsync(
		string? productNameOrBarcode,
		string? authorName,
		TimeRange timeRange,
		OrderBy orderBy,
		Pagination pagination)
	{
		var query = Database.Collection<ImportReport>();
		PipelineDefinition<ImportReport, ImportReport> pipeline = new EmptyPipelineDefinition<ImportReport>();

		var filters = new List<FilterDefinition<ImportReport>>();
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

		var sortStage = Utility.BuildStageSort<ImportReport>(x => x.DateCreated, orderBy);
		pipeline = pipeline.AppendStage(sortStage);

		var groupStage = Utility.BuildStageGroupAndPage<ImportReport>(pagination);
		var finalPipeline = pipeline.AppendStage(groupStage);

		var result = await query.Aggregate(finalPipeline).FirstOrDefaultAsync();
		result ??= new PartialEnumerable<ImportReport>([], 0);

		return result;
	}
}
