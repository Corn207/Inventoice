using Application.Exceptions;
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
		OrderBy orderBy,
		Pagination pagination)
	{
		var entities = await clientRepository.SearchAsync(nameOrPhonenumber, orderBy, pagination);

		return entities.Select(ClientMapper.ToShort);
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

	/// <summary>
	/// 
	/// </summary>
	/// <param name="id"></param>
	/// <param name="update"></param>
	/// <returns></returns>
	/// <exception cref="InvalidIdException"></exception>
	public async Task ReplaceAsync(string id, ClientCreateUpdate update)
	{
		var entity = await clientRepository.GetAsync(id) ?? throw new InvalidIdException("CliendId was not found.", [id]);
		ClientMapper.ToEntity(update, entity);
		await clientRepository.ReplaceAsync(entity);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	/// <exception cref="InvalidIdException"></exception>
	public async Task DeleteAsync(string id)
	{
		try
		{
			await clientRepository.DeleteAsync(id);
		}
		catch (InvalidIdException)
		{
			throw;
		}
	}
}
