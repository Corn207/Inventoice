using Application.Interfaces.Repositories;
using Domain.DTOs;
using Domain.Entities;
using Infrastructure.Repositories.Bases;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Infrastructure.Repositories;
public class UserRepository(Database database)
	: Repository<User>(database), IUserRepository
{
	private IMongoQueryable<User> GetSearchQuery(
		string name,
		OrderBy orderBy)
	{
		var query = Database.Collection<User>().AsQueryable();

		if (!string.IsNullOrWhiteSpace(name))
		{
			query = query.Where(p => p.Name.Contains(name, StringComparison.InvariantCultureIgnoreCase));
		}

		if (orderBy == OrderBy.Ascending)
		{
			query = query.OrderBy(x => x.Name);
		}
		else
		{
			query = query.OrderByDescending(x => x.Name);
		}

		return query;
	}

	public async Task<List<User>> SearchAsync(
		string name,
		OrderBy orderBy,
		Pagination pagination)
	{
		var query = GetSearchQuery(name, orderBy);

		var result = await query
			.Skip((pagination.PageNumber - 1) * pagination.PageSize)
			.Take(pagination.PageSize)
			.ToListAsync();

		return result;
	}

	public async Task<uint> CountAsync(
		string name,
		OrderBy orderBy)
	{
		var query = GetSearchQuery(name, orderBy);

		var result = await query.CountAsync();

		return Convert.ToUInt32(result);
	}
}
