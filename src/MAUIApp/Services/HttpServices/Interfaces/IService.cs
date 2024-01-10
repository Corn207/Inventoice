namespace MAUIApp.Services.HttpServices.Interfaces;

public interface IService<T>
{
	Task<uint> CountAllAsync(CancellationToken cancellationToken = default);
	Task<T> GetAsync(string id, CancellationToken cancellationToken = default);
	Task DeleteAsync(string id, CancellationToken cancellationToken = default);
}
