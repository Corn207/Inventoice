using Application.Interfaces.Repositories;
using Domain.DTOs;
using Domain.Entities;
using Infrastructure.Repositories.Bases;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Infrastructure.Repositories;
public class ClientRepository(Database database) : Repository<Client>(database), IClientRepository
{
	public async Task<List<Client>> SearchAsync(
		string nameOrPhonenumber,
		OrderBy orderBy,
		Pagination pagination)
	{
		var query = Database.Collection<Client>().AsQueryable();

		if (!string.IsNullOrWhiteSpace(nameOrPhonenumber))
		{
			query = query.Where(x => x.Name.Contains(nameOrPhonenumber) || x.Phonenumber.Contains(nameOrPhonenumber));
		}

		if (orderBy == OrderBy.Ascending)
		{
			query = query.OrderBy(p => p.Name);
		}
		else
		{
			query = query.OrderByDescending(p => p.Name);
		}

		var clients = await query
			.Skip((pagination.PageNumber - 1) * pagination.PageSize)
			.Take(pagination.PageSize)
			.ToListAsync();

		return clients;
	}
}
