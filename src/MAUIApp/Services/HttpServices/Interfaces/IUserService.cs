using Domain.DTOs;
using Domain.DTOs.Users;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUIApp.Services.HttpServices.Interfaces;
public interface IUserService
{
	Task<UserShort[]> SearchAsync(
		string? name = null,
		OrderBy orderBy = OrderBy.Ascending,
		ushort pageNumber = 1,
		ushort pageSize = 15,
		CancellationToken cancellationToken = default);
	Task<uint> CountAsync(
		string? name = null,
		CancellationToken cancellationToken = default);
	Task<uint> CountAllAsync(
		CancellationToken cancellationToken = default);
	Task<User> GetAsync(
		string id,
		CancellationToken cancellationToken = default);

}
