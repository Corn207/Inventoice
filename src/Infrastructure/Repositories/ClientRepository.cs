using Application.Interfaces.Repositories;
using Domain.DTOs;
using Domain.Entities;
using Infrastructure.Repositories.Bases;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Infrastructure.Repositories;
public class ClientRepository(Database database)
	: Repository<Client>(database), IClientRepository
{
	private IMongoQueryable<Client> GetSearchQuery(
		string nameOrPhonenumber,
		OrderBy orderBy)
	{
		var query = Database.Collection<Client>().AsQueryable();

		if (!string.IsNullOrWhiteSpace(nameOrPhonenumber))
		{
			query = query.Where(x =>
				x.Name.Contains(nameOrPhonenumber, StringComparison.InvariantCultureIgnoreCase) ||
				x.Phonenumber.Contains(nameOrPhonenumber, StringComparison.InvariantCultureIgnoreCase));
		}

		if (orderBy == OrderBy.Ascending)
		{
			query = query.OrderBy(p => p.Name);
		}
		else
		{
			query = query.OrderByDescending(p => p.Name);
		}

		return query;
	}

	public async Task<List<Client>> SearchAsync(
		string nameOrPhonenumber,
		OrderBy orderBy,
		Pagination pagination)
	{
		var query = GetSearchQuery(nameOrPhonenumber, orderBy);

		var result = await query
			.Skip((pagination.PageNumber - 1) * pagination.PageSize)
			.Take(pagination.PageSize)
			.ToListAsync();

		return result;
	}

	public async Task<uint> CountAsync(
		string nameOrPhonenumber,
		OrderBy orderBy)
	{
		var query = GetSearchQuery(nameOrPhonenumber, orderBy);

		var result = await query.CountAsync();

		return Convert.ToUInt32(result);
	}
}
