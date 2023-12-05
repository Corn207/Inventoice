using Application.Interfaces.Repositories.Bases;
using Domain.Entities;
using Domain.Parameters;

namespace Application.Interfaces.Repositories;
public interface IExportReportRepository : ISoftDeletableRepository<ExportReport>
{
	Task<List<ExportReport>> SearchAsync(
		string nameOrBarcode,
		PaginationParameters pagination,
		TimeRangeParameters timeRange,
		bool isDescending);
}
