using Domain.DTOs;
using Domain.DTOs.ImportReports;
using Domain.Entities;

namespace MAUIApp.Services.HttpServices.Interfaces;

public interface IImportReportService
{
	Task<IEnumerable<ImportReportShort>> SearchAsync(
		string? productNameOrBarcode = null,
		string? authorName = null,
		DateTime? dateStart = null,
		DateTime? dateEnd = null,
		OrderBy orderBy = OrderBy.Descending,
		ushort pageNumber = 1,
		ushort pageSize = 30,
		CancellationToken cancellationToken = default);

	Task<uint> TotalAsync(
		CancellationToken cancellationToken = default);

	Task<ImportReport> GetAsync(
		string id,
		CancellationToken cancellationToken = default);

	Task CreateAsync(
		ImportReportCreate body,
		CancellationToken cancellationToken = default);

	Task DeleteAsync(
		string id,
		CancellationToken cancellationToken = default);

	Task CancelAsync(
		string id,
		CancellationToken cancellationToken = default);
}
