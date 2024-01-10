using Domain.DTOs;
using Domain.DTOs.Clients;
using Domain.Entities;

namespace MAUIApp.Services.HttpServices.Interfaces;

public interface IClientService : IService<Client>
{
    Task<ClientShort[]> SearchAsync(
        string? nameOrPhonenumber = null,
        OrderBy orderBy = OrderBy.Ascending,
        ushort pageNumber = 1,
        ushort pageSize = 15,
		CancellationToken cancellationToken = default);
    Task<uint> CountAsync(
        string? nameOrPhonenumber = null,
		CancellationToken cancellationToken = default);
    Task CreateAsync(
        ClientCreateUpdate body,
		CancellationToken cancellationToken = default);
    Task UpdateAsync(
        string id,
        ClientCreateUpdate body,
        CancellationToken cancellationToken = default);
}
