using Application.Interfaces.Repositories.Bases;
using Domain.DTOs;
using Domain.Entities;

namespace Application.Interfaces.Repositories;
public interface IImportReportRepository : IRepository<ImportReport>
{
	Task<List<ImportReport>> SearchAsync(
		string? productNameOrBarcode,
		string? authorName,
		TimeRange timeRange,
		OrderBy orderBy,
		Pagination pagination);
}
