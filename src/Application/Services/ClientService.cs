using Application.Exceptions;
using Application.Interfaces.Repositories;
using Domain.DTOs;
using Domain.DTOs.Clients;
using Domain.Entities;
using Domain.Mappers;

namespace Application.Services;
public class ClientService(IClientRepository clientRepository)
{
	public async Task<PartialArray<ClientShort>> SearchAsync(
		string? nameOrPhonenumber,
		OrderBy orderBy,
		Pagination pagination)
	{
		var entities = await clientRepository.SearchAsync(
			nameOrPhonenumber,
			orderBy,
			pagination);

		return entities.ToPartialArray(ClientMapper.ToShort);
	}

	public async Task<uint> CountAllAsync()
	{
		var count = await clientRepository.CountAllAsync();

		return count;
	}

	public async Task<Client?> GetAsync(string id)
	{
		return await clientRepository.GetAsync(id);
	}

	public async Task<string> CreateAsync(ClientCreateUpdate create)
	{
		var entity = ClientMapper.ToEntity(create);
		await clientRepository.CreateAsync(entity);

		return entity.Id!;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="id"></param>
	/// <param name="update"></param>
	/// <returns></returns>
	/// <exception cref="NotFoundException"></exception>
	public async Task ReplaceAsync(string id, ClientCreateUpdate update)
	{
		var entity = await clientRepository.GetAsync(id)
			?? throw new NotFoundException("Client's Id was not found.", id);
		ClientMapper.ToEntity(update, entity);
		await clientRepository.ReplaceAsync(entity);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	/// <exception cref="NotFoundException"></exception>
	public async Task DeleteAsync(string id)
	{
		await clientRepository.DeleteAsync(id);
	}
}
