using Domain.DTOs;
using Domain.DTOs.ExportReports;
using Domain.Entities;

namespace MAUIApp.Services.HttpServices.Interfaces;

public interface IExportReportService
{
	Task<IEnumerable<ExportReportShort>> SearchAsync(
		string? productNameOrBarcode = null,
		string? authorName = null,
		DateTime? dateStart = null,
		DateTime? dateEnd = null,
		OrderBy orderBy = OrderBy.Descending,
		ushort pageNumber = 1,
		ushort pageSize = 15,
		CancellationToken cancellationToken = default);

	Task<uint> TotalAsync(
		CancellationToken cancellationToken = default);

	Task<ExportReport> GetAsync(
		string id,
		CancellationToken cancellationToken = default);

	Task CreateAsync(
		ExportReportCreate body,
		CancellationToken cancellationToken = default);

	Task DeleteAsync(
		string id,
		CancellationToken cancellationToken = default);

	Task CancelAsync(
		string id,
		CancellationToken cancellationToken = default);
}
