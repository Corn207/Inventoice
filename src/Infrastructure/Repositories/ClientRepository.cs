using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Parameters;
using Infrastructure.Repositories.Bases;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Infrastructure.Repositories;
public class ClientRepository : Repository<Client>, IClientRepository
{
	public ClientRepository(Database database) : base(database)
	{
	}

	public async Task<List<Client>> SearchAsync(
		string nameOrPhonenumber,
		PaginationParameters pagination,
		bool isDescending)
	{
		var query = Database.Collection<Client>().AsQueryable();

		if (!string.IsNullOrWhiteSpace(nameOrPhonenumber))
		{
			query = query.Where(x => x.Name.Contains(nameOrPhonenumber) || x.PhoneNumber.Contains(nameOrPhonenumber));
		}

		if (isDescending)
		{
			query = query.OrderByDescending(p => p.Name);
		}
		else
		{
			query = query.OrderBy(p => p.Name);
		}

		var clients = await query
			.Skip((pagination.PageNumber - 1) * pagination.PageSize)
			.Take(pagination.PageSize)
			.ToListAsync();

		return clients;
	}
}
