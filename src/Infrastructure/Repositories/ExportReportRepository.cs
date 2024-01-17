using Application.Interfaces.Repositories;
using Domain.DTOs;
using Domain.Entities;
using Infrastructure.Repositories.Bases;
using MongoDB.Driver;

namespace Infrastructure.Repositories;
public class ExportReportRepository(Database database)
	: Repository<ExportReport>(database), IExportReportRepository
{
	public async Task<List<ExportReport>> SearchAsync(
		string? productNameOrBarcode,
		string? authorName,
		TimeRange timeRange,
		OrderBy orderBy,
		Pagination pagination)
	{
		var query = Database.Collection<ExportReport>();
		PipelineDefinition<ExportReport, ExportReport> pipeline = new EmptyPipelineDefinition<ExportReport>();

		var filters = new List<FilterDefinition<ExportReport>>();
		filters.AddTextSearchCaseInsentitive(
			x => x.ProductItems,
			(x => x.Name, productNameOrBarcode),
			(x => x.Barcode, productNameOrBarcode));
		filters.AddTextSearchCaseInsentitive((x => x.Author.Name, authorName));
		filters.AddFilterTimeRange(x => x.DateCreated, timeRange);

		var result = await Database.Collection<ExportReport>()
			.And(filters)
			.Sort(x => x.DateCreated, orderBy)
			.Paginate(pagination)
			.ToListAsync();

		return result;
	}
}
