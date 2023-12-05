using Application.Interfaces.Repositories;
using Domain.DTOs.Clients;
using Domain.Entities;
using Domain.Mappers;
using Domain.Parameters;

namespace Application.Services;
public class ClientService
{
	private readonly IClientRepository _clientRepository;

	public ClientService(IClientRepository clientRepository)
	{
		_clientRepository = clientRepository;
	}

	public async Task<IEnumerable<ClientShort>> SearchAsync(
		string? nameOrPhonenumber = null,
		ushort pageNumber = 1,
		ushort pageSize = 15,
		bool isDescending = false)
	{
		var pagination = new PaginationParameters(pageNumber, pageSize);
		var entities = await _clientRepository.SearchAsync(nameOrPhonenumber ?? string.Empty, pagination, isDescending);

		return entities.Select(ClientMapper.ToShortForm);
	}

	public async Task<Client?> GetAsync(string id)
	{
		return await _clientRepository.GetAsync(id);
	}

	public async Task<string> CreateAsync(ClientCreateUpdate create)
	{
		var entity = ClientMapper.ToEntity(create);
		entity.DateCreated = DateTime.Now;
		await _clientRepository.CreateAsync(entity);

		return entity.Id!;
	}

	public async Task ReplaceAsync(string id, ClientCreateUpdate update)
	{
		var entity = await _clientRepository.GetAsync(id) ?? throw new KeyNotFoundException();
		ClientMapper.ToEntity(update, entity);
		await _clientRepository.ReplaceAsync(entity);
	}

	public async Task DeleteAsync(string id)
	{
		try
		{
			await _clientRepository.DeleteAsync(id);
		}
		catch (KeyNotFoundException ex)
		{
			throw ex;
		}
	}
}
