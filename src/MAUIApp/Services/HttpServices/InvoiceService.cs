using Domain.DTOs;
using Domain.DTOs.Invoices;
using Domain.Entities;
using MAUIApp.Services.HttpServices.Interfaces;

namespace MAUIApp.Services.HttpServices;
public class InvoiceService(HttpService httpService) : BaseService<Invoice>, IInvoiceService
{
	protected override HttpService HttpService => httpService;
	protected override string Path { get; } = "Invoices";

	public async Task<InvoiceShort[]> SearchAsync(
		string? productNameOrBarcode = null,
		string? clientNameOrPhonenumber = null,
		string? authorName = null,
		InvoiceStatus status = InvoiceStatus.All,
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
			{ nameof(clientNameOrPhonenumber), clientNameOrPhonenumber },
			{ nameof(authorName), authorName },
			{ nameof(status), status },
			{ nameof(dateStart), dateStart },
			{ nameof(dateEnd), dateEnd },
			{ nameof(orderBy), orderBy },
			{ nameof(pageNumber), pageNumber },
			{ nameof(pageSize), pageSize },
		};
		var results = await httpService.GetAsync<InvoiceShort[]>(Path, queries, cancellationToken);
		return results;
	}

	public async Task<uint> CountAsync(
		string? productNameOrBarcode = null,
		string? clientNameOrPhonenumber = null,
		string? authorName = null,
		InvoiceStatus status = InvoiceStatus.All,
		DateTime? dateStart = null,
		DateTime? dateEnd = null,
		CancellationToken cancellationToken = default)
	{
		var queries = new Dictionary<string, object?>()
		{
			{ nameof(productNameOrBarcode), productNameOrBarcode },
			{ nameof(clientNameOrPhonenumber), clientNameOrPhonenumber },
			{ nameof(authorName), authorName },
			{ nameof(status), status },
			{ nameof(dateStart), dateStart },
			{ nameof(dateEnd), dateEnd },
		};
		var results = await httpService.GetAsync<uint>($"{Path}/count", queries, cancellationToken);
		return results;
	}

	public async Task CreateAsync(
		InvoiceCreate body,
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

	public async Task PayAsync(
		string id,
		CancellationToken cancellationToken = default)
	{
		await httpService.PatchNoContentAsync($"{Path}/{id}/pay", cancellationToken);
	}
}
