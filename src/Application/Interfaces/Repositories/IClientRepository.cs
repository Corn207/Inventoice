using Application.Interfaces.Repositories.Bases;
using Domain.DTOs;
using Domain.Entities;

namespace Application.Interfaces.Repositories;
public interface IClientRepository : IRepository<Client>
{
	Task<PartialEnumerable<Client>> SearchAsync(
		string? nameOrPhonenumber,
		OrderBy orderBy,
		Pagination pagination);
}
