namespace MAUIApp.Services.HttpServices.Interfaces;

public interface ICancellableService
{
	Task CancelAsync(string id, CancellationToken cancellationToken = default);
}
