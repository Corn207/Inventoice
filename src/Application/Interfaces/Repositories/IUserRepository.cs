using Application.Interfaces.Repositories.Bases;
using Domain.DTOs;
using Domain.Entities;

namespace Application.Interfaces.Repositories;
public interface IUserRepository : IRepository<User>
{
	Task<PartialEnumerable<User>> SearchAsync(
		string? name,
		OrderBy orderBy,
		Pagination pagination);
}
