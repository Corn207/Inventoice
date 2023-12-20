using Application.Interfaces.Repositories;
using Domain.DTOs;
using Domain.Entities;
using Infrastructure.Repositories.Bases;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Infrastructure.Repositories;
public class ImportReportRepository(Database database) : SoftDeletableRepository<ImportReport>(database), IImportReportRepository
{
	public async Task<List<ImportReport>> SearchAsync(
		string productNameOrBarcode,
		TimeRange timeRange,
		OrderBy orderBy,
		Pagination pagination)
	{
		var query = Database.Collection<ImportReport>().AsQueryable()
			.Where(x => x.DateDeleted == null);

		if (!string.IsNullOrWhiteSpace(productNameOrBarcode))
		{
			query = query.Where(x => x.ProductItems.Any(i => i.Name.Contains(productNameOrBarcode) || i.Barcode.Contains(productNameOrBarcode)));
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
