using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.Parameters;
using Domain.Entities;
using Infrastructure.Repositories.Bases;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Infrastructure.Repositories;
public class AuditReportRepository : SoftDeletableRepository<AuditReport>, IAuditReportRepository
{
	public AuditReportRepository(Database database) : base(database)
	{
	}

	public async Task<List<AuditReport>> SearchAsync(
		string nameOrBarcode,
		PaginationParameters pagination,
		TimeRangeParameters timeRange,
		bool isDescending)
	{
		var query = Database.Collection<AuditReport>().AsQueryable()
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

		if (isDescending)
		{
			query = query.OrderByDescending(p => p.DateCreated);
		}
		else
		{
			query = query.OrderBy(p => p.DateCreated);
		}

		var entity = await query
			.Skip((pagination.PageNumber - 1) * pagination.PageSize)
			.Take(pagination.PageSize)
			.ToListAsync();

		return entity;
	}
}
