using Application.Interfaces.Repositories;
using Domain.DTOs;
using Domain.Entities;
using Infrastructure.Repositories.Bases;
using MongoDB.Driver;

namespace Infrastructure.Repositories;
public class ClientRepository(Database database)
	: Repository<Client>(database), IClientRepository
{
	public async Task<List<Client>> SearchAsync(
		string? nameOrPhonenumber,
		OrderBy orderBy,
		Pagination pagination)
	{
		var filters = new List<FilterDefinition<Client>>();
		filters.AddTextSearchCaseInsentitive(
			(x => x.Name, nameOrPhonenumber),
			(x => x.Phonenumber, nameOrPhonenumber));

		var result = await Database.Collection<Client>()
			.And(filters)
			.Sort(x => x.DateCreated, orderBy)
			.Paginate(pagination)
			.ToListAsync();

		return result;
	}
}
