using Application.Interfaces.Repositories;
using Domain.DTOs;
using Domain.Entities;
using Infrastructure.Repositories.Bases;
using MongoDB.Driver;

namespace Infrastructure.Repositories;
public class AuditReportRepository(Database database)
	: Repository<AuditReport>(database), IAuditReportRepository
{
	public async Task<List<AuditReport>> SearchAsync(
		string? productNameOrBarcode,
		string? authorName,
		TimeRange timeRange,
		OrderBy orderBy,
		Pagination pagination)
	{
		var filters = new List<FilterDefinition<AuditReport>>();
		filters.AddTextSearchCaseInsentitive(
			x => x.ProductItems,
			(x => x.Name, productNameOrBarcode),
			(x => x.Barcode, productNameOrBarcode));
		filters.AddTextSearchCaseInsentitive((x => x.Author.Name, authorName));
		filters.AddFilterTimeRange(x => x.DateCreated, timeRange);

		var result = await Database.Collection<AuditReport>()
			.And(filters)
			.Sort(x => x.DateCreated, orderBy)
			.Paginate(pagination)
			.ToListAsync();

		return result;
	}
}
