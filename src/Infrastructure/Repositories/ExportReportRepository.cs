using Application.Interfaces.Repositories;
using Domain.DTOs;
using Domain.Entities;
using Infrastructure.Repositories.Bases;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Infrastructure.Repositories;
public class ExportReportRepository(Database database) : SoftDeletableRepository<ExportReport>(database), IExportReportRepository
{
	public async Task<List<ExportReport>> SearchAsync(
		string nameOrBarcode,
		Pagination pagination,
		TimeRange timeRange,
		OrderBy orderBy)
	{
		var query = Database.Collection<ExportReport>().AsQueryable()
			.Where(x => x.DateDeleted == null);

		if (!string.IsNullOrWhiteSpace(nameOrBarcode))
		{
			query = query.Where(x => x.ProductItems.Any(i => i.Name.Contains(nameOrBarcode) || i.Barcode.Contains(nameOrBarcode)));
		}

		if (timeRange.From != DateTime.MinValue)
		{
			query = query.Where(p => p.DateCreated >= timeRange.From);
		}

		if (timeRange.To != DateTime.MaxValue)
		{
			query = query.Where(p => p.DateCreated <= timeRange.To);
		}

		if (orderBy == OrderBy.Ascending)
		{
			query = query.OrderBy(p => p.DateCreated);
		}
		else
		{
			query = query.OrderByDescending(p => p.DateCreated);
		}

		var entity = await query
			.Skip((pagination.PageNumber - 1) * pagination.PageSize)
			.Take(pagination.PageSize)
			.ToListAsync();

		return entity;
	}
}
