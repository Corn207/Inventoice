using Application.Interfaces.Repositories;
using Domain.DTOs;
using Domain.DTOs.Clients;
using Domain.Entities;
using Domain.Mappers;

namespace Application.Services;
public class ClientService(IClientRepository clientRepository)
{
	public async Task<IEnumerable<ClientShort>> SearchAsync(
		string nameOrPhonenumber,
		ushort pageNumber,
		ushort pageSize,
		OrderBy orderBy)
	{
		var pagination = new Pagination(pageNumber, pageSize);
		var entities = await clientRepository.SearchAsync(nameOrPhonenumber, pagination, orderBy);

		return entities.Select(ClientMapper.ToShortForm);
	}

	public async Task<Client?> GetAsync(string id)
	{
		return await clientRepository.GetAsync(id);
	}

	public async Task<string> CreateAsync(ClientCreateUpdate create)
	{
		var entity = ClientMapper.ToEntity(create);
		entity.DateCreated = DateTime.Now;
		await clientRepository.CreateAsync(entity);

		return entity.Id!;
	}

	public async Task ReplaceAsync(string id, ClientCreateUpdate update)
	{
		var entity = await clientRepository.GetAsync(id) ?? throw new KeyNotFoundException();
		ClientMapper.ToEntity(update, entity);
		await clientRepository.ReplaceAsync(entity);
	}

	public async Task DeleteAsync(string id)
	{
		try
		{
			await clientRepository.DeleteAsync(id);
		}
		catch (KeyNotFoundException)
		{
			throw;
		}
	}
}
