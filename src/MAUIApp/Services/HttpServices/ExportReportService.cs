using Domain.DTOs;
using Domain.DTOs.ExportReports;
using Domain.Entities;
using MAUIApp.Services.HttpServices.Interfaces;

namespace MAUIApp.Services.HttpServices;
public class ExportReportService(HttpService httpService) : BaseService<ExportReport>, IExportReportService
{
	protected override HttpService HttpService => httpService;
	protected override string Path { get; } = "ExportReports";

	public async Task<ExportReportShort[]> SearchAsync(
		string? productNameOrBarcode = null,
		string? authorName = null,
		DateTime? dateStart = null,
		DateTime? dateEnd = null,
		OrderBy orderBy = OrderBy.Descending,
		ushort pageNumber = 1,
		ushort pageSize = 15,
		CancellationToken cancellationToken = default)
	{
		var queries = new Dictionary<string, object?>()
		{
			{ nameof(productNameOrBarcode), productNameOrBarcode },
			{ nameof(authorName), authorName },
			{ nameof(dateStart), dateStart },
			{ nameof(dateEnd), dateEnd },
			{ nameof(orderBy), orderBy },
			{ nameof(pageNumber), pageNumber },
			{ nameof(pageSize), pageSize },
		};
		var results = await httpService.GetAsync<ExportReportShort[]>(Path, queries, cancellationToken);
		return results;
	}

	public async Task<uint> CountAsync(
		string? productNameOrBarcode = null,
		string? authorName = null,
		DateTime? dateStart = null,
		DateTime? dateEnd = null,
		CancellationToken cancellationToken = default)
	{
		var queries = new Dictionary<string, object?>()
		{
			{ nameof(productNameOrBarcode), productNameOrBarcode },
			{ nameof(authorName), authorName },
			{ nameof(dateStart), dateStart },
			{ nameof(dateEnd), dateEnd },
		};
		var results = await httpService.GetAsync<uint>($"{Path}/count", queries, cancellationToken);
		return results;
	}

	public async Task CreateAsync(
		ExportReportCreate body,
		CancellationToken cancellationToken = default)
	{
		await httpService.PostNoContentAsync(Path, body, cancellationToken);
	}

	public async Task CancelAsync(
		string id,
		CancellationToken cancellationToken = default)
	{
		await httpService.PatchNoContentAsync($"{Path}/{id}/cancel", cancellationToken);
	}
}
