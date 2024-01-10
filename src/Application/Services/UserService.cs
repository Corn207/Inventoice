using Application.Exceptions;
using Application.Interfaces.Repositories;
using Domain.DTOs;
using Domain.DTOs.Users;
using Domain.Entities;
using Domain.Mappers;

namespace Application.Services;
public class UserService(IUserRepository userRepository)
{
	public async Task<IEnumerable<UserShort>> SearchAsync(
		string name,
		OrderBy orderBy,
		Pagination pagination)
	{
		var entities = await userRepository.SearchAsync(name, orderBy, pagination);

		return entities.Select(UserMapper.ToShort);
	}

	public async Task<uint> CountAsync(
		string name)
	{
		var count = await userRepository.CountAsync(name);

		return count;
	}

	public async Task<uint> CountAllAsync()
	{
		var count = await userRepository.CountAllAsync();

		return count;
	}

	public async Task<User?> GetAsync(string id)
	{
		return await userRepository.GetAsync(id);
	}

	public async Task<string> CreateAsync(UserCreateUpdate create)
	{
		var entity = UserMapper.ToEntity(create);
		await userRepository.CreateAsync(entity);

		return entity.Id!;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="id"></param>
	/// <param name="update"></param>
	/// <returns></returns>
	/// <exception cref="InvalidIdException"></exception>
	public async Task ReplaceAsync(string id, UserCreateUpdate update)
	{
		var entity = await userRepository.GetAsync(id)
			?? throw new InvalidIdException("UserId was not found.", [id]);
		UserMapper.ToEntity(update, entity);
		await userRepository.ReplaceAsync(entity);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	/// <exception cref="InvalidIdException"></exception>
	public async Task DeleteAsync(string id)
	{
		await userRepository.DeleteAsync(id);
	}
}
