using Domain.DTOs;
using Domain.DTOs.Users;
using Domain.Entities;
using MAUIApp.Services.HttpServices.Interfaces;
using MAUIApp.Utilities;

namespace MAUIApp.Services.HttpServices;
public class UserService(HttpService httpService) : IUserService
{
	private const string _path = "Users";

	public async Task<IEnumerable<UserShort>> SearchAsync(
		string? name = null,
		OrderBy orderBy = OrderBy.Ascending,
		ushort pageNumber = 1,
		ushort pageSize = 15,
		CancellationToken cancellationToken = default)
	{
		var queries = new Dictionary<string, object?>()
		{
			{ nameof(name), name },
			{ nameof(orderBy), orderBy },
			{ nameof(pageNumber), pageNumber },
			{ nameof(pageSize), pageSize },
		};
		var queryString = QueryStringConverter.Convert(queries);
		var uri = new Uri(httpService.BaseUri, $"{_path}/search?{queryString}");
		var results = await httpService.GetAsync<IEnumerable<UserShort>>(uri, cancellationToken);

		return results;
	}

	public async Task<uint> TotalAsync(
		CancellationToken cancellationToken = default)
	{
		var uri = new Uri(httpService.BaseUri, $"{_path}/total");
		var results = await httpService.GetAsync<uint>(uri, cancellationToken);

		return results;
	}

	public async Task<User> GetAsync(
		string id,
		CancellationToken cancellationToken = default)
	{
		var uri = new Uri(httpService.BaseUri, $"{_path}/{id}");
		var result = await httpService.GetAsync<User>(uri, cancellationToken);

		return result;
	}

	public async Task<User> GetMeAsync(
		CancellationToken cancellationToken = default)
	{
		var uri = new Uri(httpService.BaseUri, $"{_path}/me");
		var result = await httpService.GetAsync<User>(uri, cancellationToken);

		return result;
	}

	public async Task EditMeAsync(
		UserCreateUpdate update,
		CancellationToken cancellationToken = default)
	{
		var uri = new Uri(httpService.BaseUri, $"{_path}/me");
		await httpService.PutAsync(uri, update, cancellationToken);
	}
}
