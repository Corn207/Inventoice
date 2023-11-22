using Application.Interfaces.Repositories.Bases;
using Application.Interfaces.Repositories.Parameters;
using Domain.Entities;

namespace Application.Interfaces.Repositories;
public interface IAuditReportRepository : ISoftDeletableRepository<AuditReport>
{
	Task<List<AuditReport>> SearchAsync(
		string nameOrBarcode,
		PaginationParameters pagination,
		TimeRangeParameters timeRange,
		bool isDescending);
}
