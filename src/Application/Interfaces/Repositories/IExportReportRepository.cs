using Application.Interfaces.Repositories.Bases;
using Domain.DTOs;
using Domain.Entities;

namespace Application.Interfaces.Repositories;
public interface IExportReportRepository : ISoftDeletableRepository<ExportReport>
{
	Task<List<ExportReport>> SearchAsync(
		string productNameOrBarcode,
		string authorName,
		TimeRange timeRange,
		OrderBy orderBy,
		Pagination pagination);
	Task<uint> CountAsync(
		string productNameOrBarcode,
		string authorName,
		TimeRange timeRange);
}
