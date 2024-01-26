using Domain.DTOs;
using Domain.DTOs.ImportReports;
using Domain.Entities;
using MAUIApp.Services.HttpServices.Interfaces;
using MAUIApp.Utilities;

namespace MAUIApp.Services.HttpServices;
public class ImportReportService(HttpService httpService) : IImportReportService
{
	private const string _path = "ImportReports";

	public async Task<IEnumerable<ImportReportShort>> SearchAsync(
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
		var queryString = QueryStringConverter.Convert(queries);
		var uri = new Uri(httpService.BaseUri, $"{_path}/search?{queryString}");
		var results = await httpService.GetAsync<IEnumerable<ImportReportShort>>(uri, cancellationToken);

		return results;
	}

	public async Task<uint> TotalAsync(
		CancellationToken cancellationToken = default)
	{
		var uri = new Uri(httpService.BaseUri, $"{_path}/total");
		var results = await httpService.GetAsync<uint>(uri, cancellationToken);

		return results;
	}

	public async Task<ImportReport> GetAsync(
		string id,
		CancellationToken cancellationToken = default)
	{
		var uri = new Uri(httpService.BaseUri, $"{_path}/{id}");
		var result = await httpService.GetAsync<ImportReport>(uri, cancellationToken);

		return result;
	}

	public async Task CreateAsync(
		ImportReportCreate body,
		CancellationToken cancellationToken = default)
	{
		var uri = new Uri(httpService.BaseUri, $"{_path}");
		await httpService.PostNoContentAsync(uri, body, cancellationToken);
	}

	public async Task DeleteAsync(
		string id,
		CancellationToken cancellationToken = default)
	{
		var uri = new Uri(httpService.BaseUri, $"{_path}/{id}");
		await httpService.DeleteAsync(uri, cancellationToken);
	}

	public async Task CancelAsync(
		string id,
		CancellationToken cancellationToken = default)
	{
		var uri = new Uri(httpService.BaseUri, $"{_path}/{id}/cancel");
		await httpService.PatchNoContentAsync(uri, cancellationToken);
	}
}
