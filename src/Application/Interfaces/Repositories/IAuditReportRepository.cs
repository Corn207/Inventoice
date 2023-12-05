using Application.Interfaces.Repositories.Bases;
using Domain.Entities;
using Domain.Parameters;

namespace Application.Interfaces.Repositories;
public interface IAuditReportRepository : ISoftDeletableRepository<AuditReport>
{
	Task<List<AuditReport>> SearchAsync(
		string nameOrBarcode,
		PaginationParameters pagination,
		TimeRangeParameters timeRange,
		bool isDescending);
}
