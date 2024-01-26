using Application.Interfaces.Repositories;
using Domain.DTOs;
using Domain.Entities;
using Infrastructure.Repositories.Bases;
using MongoDB.Driver;

namespace Infrastructure.Repositories;
public class UserRepository(Database database)
	: Repository<User>(database), IUserRepository
{
	public async Task<List<User>> SearchAsync(
		string? name,
		OrderBy orderBy,
		Pagination pagination)
	{
		var filters = new List<FilterDefinition<User>>();
		filters.AddTextSearchCaseInsentitive((x => x.Name, name));

		var result = await Database.Collection<User>()
			.And(filters)
			.Sort(x => x.Name, orderBy)
			.Paginate(pagination)
			.ToListAsync();

		return result;
	}
}
