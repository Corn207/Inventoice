using Domain.DTOs;
using Domain.DTOs.Clients;
using Domain.Entities;
using MAUIApp.Services.HttpServices.Interfaces;

namespace MAUIApp.Services.HttpServices;
public class ClientService(HttpService httpService) : BaseService<Client>, IClientService
{
	protected override HttpService HttpService => httpService;
	protected override string Path { get; } = "Clients";

	public async Task<ClientShort[]> SearchAsync(
		string? nameOrPhonenumber = null,
		OrderBy orderBy = OrderBy.Descending,
		ushort pageNumber = 1,
		ushort pageSize = 15,
		CancellationToken cancellationToken = default)
	{
		var queries = new Dictionary<string, object?>()
		{
			{ nameof(nameOrPhonenumber), nameOrPhonenumber },
			{ nameof(orderBy), orderBy },
			{ nameof(pageNumber), pageNumber },
			{ nameof(pageSize), pageSize },
		};
		var results = await httpService.GetAsync<ClientShort[]>(Path, queries, cancellationToken);
		return results;
	}

	public async Task<uint> CountAsync(
		string? nameOrPhonenumber = null,
		CancellationToken cancellationToken = default)
	{
		var queries = new Dictionary<string, object?>()
		{
			{ nameof(nameOrPhonenumber), nameOrPhonenumber },
		};
		var results = await httpService.GetAsync<uint>($"{Path}/count", queries, cancellationToken);
		return results;
	}

	public async Task CreateAsync(
		ClientCreateUpdate body,
		CancellationToken cancellationToken = default)
	{
		await httpService.PostNoContentAsync(Path, body, cancellationToken);
	}

	public async Task UpdateAsync(
		string id,
		ClientCreateUpdate body)
	{
		await UpdateAsync(id, body, CancellationToken.None);
	}

	public async Task UpdateAsync(
		string id,
		ClientCreateUpdate body,
		CancellationToken cancellationToken = default)
	{
		await httpService.PutNoContentAsync($"{Path}/{id}", body, cancellationToken);
	}
}
