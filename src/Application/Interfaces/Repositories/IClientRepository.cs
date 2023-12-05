using Application.Interfaces.Repositories.Bases;
using Domain.Entities;
using Domain.Parameters;

namespace Application.Interfaces.Repositories;
public interface IClientRepository : IRepository<Client>
{
	Task<List<Client>> SearchAsync(
		string nameOrPhonenumber,
		PaginationParameters pagination,
		bool isDescending);
}
