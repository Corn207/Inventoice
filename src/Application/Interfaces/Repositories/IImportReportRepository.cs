using Application.Interfaces.Repositories.Bases;
using Application.Interfaces.Repositories.Parameters;
using Domain.Entities;

namespace Application.Interfaces.Repositories;
public interface IImportReportRepository : ISoftDeletableRepository<ImportReport>
{
	Task<List<ImportReport>> SearchAsync(
		string nameOrBarcode,
		PaginationParameters pagination,
		TimeRangeParameters timeRange,
		bool isDescending);
}
