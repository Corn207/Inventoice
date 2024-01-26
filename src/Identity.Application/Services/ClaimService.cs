using Identity.Application.Exceptions;
using Identity.Domain.Entity;
using MongoDB.Driver;

namespace Identity.Application.Services;
public class ClaimService(IdentityDatabase database)
{
	public async Task UpdateRolesAsync(string userId, Role roles)
	{
		var updateDefinition = Builders<UserClaim>.Update.Set(x => x.Roles, roles);
		var result = await database.UserClaims.UpdateOneAsync(x => x.UserId == userId, updateDefinition);

		if (result.MatchedCount != 1)
			throw new EntityNotFoundException();
	}

	public async Task<bool> IsExistsAdminAsync()
	{
		var isExist = await database.UserClaims
			.Find(x => x.Roles.HasFlag(Role.Admin))
			.AnyAsync();

		return isExist;
	}
}
