using Application.Interfaces.Repositories.Bases;
using Domain.Entities;
using Domain.Parameters;

namespace Application.Interfaces.Repositories;
public interface IImportReportRepository : ISoftDeletableRepository<ImportReport>
{
	Task<List<ImportReport>> SearchAsync(
		string nameOrBarcode,
		PaginationParameters pagination,
		TimeRangeParameters timeRange,
		bool isDescending);
}
