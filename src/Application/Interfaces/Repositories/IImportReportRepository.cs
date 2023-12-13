using Application.Interfaces.Repositories.Bases;
using Domain.DTOs;
using Domain.Entities;

namespace Application.Interfaces.Repositories;
public interface IImportReportRepository : ISoftDeletableRepository<ImportReport>
{
	Task<List<ImportReport>> SearchAsync(
		string nameOrBarcode,
		Pagination pagination,
		TimeRange timeRange,
		OrderBy orderBy);
}
