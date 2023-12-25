using Application.Interfaces.Repositories.Bases;
using Domain.DTOs;
using Domain.Entities;

namespace Application.Interfaces.Repositories;
public interface IClientRepository : IRepository<Client>
{
	Task<List<Client>> SearchAsync(
		string nameOrPhonenumber,
		OrderBy orderBy,
		Pagination pagination);
	Task<uint> CountAsync(
		string nameOrPhonenumber);
}
