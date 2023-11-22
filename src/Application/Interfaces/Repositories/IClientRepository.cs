using Application.Interfaces.Repositories.Bases;
using Application.Interfaces.Repositories.Parameters;
using Domain.Entities;

namespace Application.Interfaces.Repositories;
public interface IClientRepository : IRepository<Client>
{
	Task<List<Client>> SearchAsync(
		string nameOrPhonenumber,
		PaginationParameters pagination,
		bool isDescending);
}
