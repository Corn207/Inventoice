using Domain.DTOs;
using Domain.DTOs.Clients;
using Domain.Entities;
using MAUIApp.Services.HttpServices.Interfaces;
using MAUIApp.Utilities;

namespace MAUIApp.Services.HttpServices;
public class ClientService(HttpService httpService) : IClientService
{
	private const string _path = "Clients";

	public async Task<IEnumerable<ClientShort>> SearchAsync(
		string? nameOrPhonenumber = null,
		OrderBy orderBy = OrderBy.Ascending,
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
		var queryString = QueryStringConverter.Convert(queries);
		var uri = new Uri(httpService.BaseUri, $"{_path}/search?{queryString}");
		var results = await httpService.GetAsync<IEnumerable<ClientShort>>(uri, cancellationToken);

		return results;
	}

	public async Task<uint> TotalAsync(
		CancellationToken cancellationToken = default)
	{
		var uri = new Uri(httpService.BaseUri, $"{_path}/total");
		var results = await httpService.GetAsync<uint>(uri, cancellationToken);

		return results;
	}

	public async Task<Client> GetAsync(
		string id,
		CancellationToken cancellationToken = default)
	{
		var uri = new Uri(httpService.BaseUri, $"{_path}/{id}");
		var result = await httpService.GetAsync<Client>(uri, cancellationToken);

		return result;
	}

	public async Task CreateAsync(
		ClientCreateUpdate body,
		CancellationToken cancellationToken = default)
	{
		var uri = new Uri(httpService.BaseUri, $"{_path}");
		await httpService.PostNoContentAsync(uri, body, cancellationToken);
	}

	public async Task UpdateAsync(
		string id,
		ClientCreateUpdate body,
		CancellationToken cancellationToken = default)
	{
		var uri = new Uri(httpService.BaseUri, $"{_path}/{id}");
		await httpService.PutNoContentAsync(uri, body, cancellationToken);
	}

	public async Task DeleteAsync(
		string id,
		CancellationToken cancellationToken = default)
	{
		var uri = new Uri(httpService.BaseUri, $"{_path}/{id}");
		await httpService.DeleteAsync(uri, cancellationToken);
	}
}
