using MAUIApp.Services.HttpServices.Interfaces;

namespace MAUIApp.Services.HttpServices;
public abstract class BaseService<T> : IService<T> where T : notnull
{
	protected abstract HttpService HttpService { get; }
	protected abstract string Path { get; }

	public async Task<uint> CountAllAsync(
		CancellationToken cancellationToken = default)
	{
		var result = await HttpService.GetAsync<uint>($"{Path}/count/all", cancellationToken: cancellationToken);
		return result;
	}

	public async Task<T> GetAsync(
		string id,
		CancellationToken cancellationToken = default)
	{
		var result = await HttpService.GetAsync<T>($"{Path}/{id}", cancellationToken: cancellationToken);
		return result;
	}

	public async Task DeleteAsync(
		string id,
		CancellationToken cancellationToken = default)
	{
		await HttpService.DeleteAsync($"{Path}/{id}", cancellationToken);
	}
}
