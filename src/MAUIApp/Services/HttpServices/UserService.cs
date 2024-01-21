using Domain.DTOs;
using Domain.DTOs.Users;
using Domain.Entities;

namespace MAUIApp.Services.HttpServices;
public class UserService(HttpService httpService)
{
	private const string _path = "Users";

	public async Task<UserShort[]> SearchAsync(
		string? name = null,
		OrderBy orderBy = OrderBy.Ascending,
		ushort pageNumber = 1,
		ushort pageSize = 15,
		CancellationToken cancellationToken = default)
	{
		var path = $"{_path}/search";

		var queries = new Dictionary<string, object?>()
		{
			{ nameof(name), name },
			{ nameof(orderBy), orderBy },
			{ nameof(pageNumber), pageNumber },
			{ nameof(pageSize), pageSize },
		};
		var results = await httpService.GetAsync<UserShort[]>(path, queries, cancellationToken);
		return results;
	}

	public async Task<uint> CountAsync(
		string? name = null,
		CancellationToken cancellationToken = default)
	{
		var path = $"{_path}/count";

		var queries = new Dictionary<string, object?>()
		{
			{ nameof(name), name },
		};
		var results = await httpService.GetAsync<uint>(path, queries, cancellationToken);
		return results;
	}

	public async Task<User> GetAsync(
		string id,
		CancellationToken cancellationToken = default)
	{
		var path = $"{_path}/{id}";

		var result = await httpService.GetAsync<User>(path, null, cancellationToken);
		return result;
	}

	public async Task<User> GetMeAsync(
		CancellationToken cancellationToken = default)
	{
		var path = $"{_path}/me";

		var result = await httpService.GetAsync<User>(path, null, cancellationToken);
		return result;
	}

	public async Task EditMeAsync(
		UserCreateUpdate update,
		CancellationToken cancellationToken = default)
	{
		var path = $"{_path}/me";

		await httpService.PutAsync(path, update, cancellationToken);
	}
}
