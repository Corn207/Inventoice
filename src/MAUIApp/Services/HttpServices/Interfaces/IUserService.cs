using Domain.DTOs;
using Domain.DTOs.Users;
using Domain.Entities;

namespace MAUIApp.Services.HttpServices.Interfaces;
public interface IUserService
{
	Task<IEnumerable<UserShort>> SearchAsync(
		string? name = null,
		OrderBy orderBy = OrderBy.Ascending,
		ushort pageNumber = 1,
		ushort pageSize = 15,
		CancellationToken cancellationToken = default);

	Task<uint> TotalAsync(
		CancellationToken cancellationToken = default);

	Task<User> GetAsync(
		string id,
		CancellationToken cancellationToken = default);

	Task<User> GetMeAsync(
		CancellationToken cancellationToken = default);

	Task EditMeAsync(
		UserCreateUpdate update,
		CancellationToken cancellationToken = default);
}
