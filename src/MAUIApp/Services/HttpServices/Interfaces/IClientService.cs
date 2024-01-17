using Domain.DTOs;
using Domain.DTOs.Clients;
using Domain.Entities;

namespace MAUIApp.Services.HttpServices.Interfaces;

public interface IClientService
{
	Task<IEnumerable<ClientShort>> SearchAsync(
		string? nameOrPhonenumber = null,
		OrderBy orderBy = OrderBy.Ascending,
		ushort pageNumber = 1,
		ushort pageSize = 30,
		CancellationToken cancellationToken = default);

	Task<uint> TotalAsync(
		CancellationToken cancellationToken = default);

	Task<Client> GetAsync(
		string id,
		CancellationToken cancellationToken = default);

	Task CreateAsync(
		ClientCreateUpdate body,
		CancellationToken cancellationToken = default);

	Task UpdateAsync(
		string id,
		ClientCreateUpdate body,
		CancellationToken cancellationToken = default);

	Task DeleteAsync(
		string id,
		CancellationToken cancellationToken = default);
}
