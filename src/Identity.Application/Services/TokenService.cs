using Identity.Application.Exceptions;
using Identity.Domain.DTOs;
using Identity.Domain.Entity;
using MongoDB.Driver;
using System.Security.Claims;

namespace Identity.Application.Services;
public class TokenService(IdentityDatabase database)
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="token"></param>
	/// <returns></returns>
	/// <exception cref="EntityNotFoundException"></exception>
	/// <exception cref="DatabaseOperationErrorException"></exception>
	public async Task<List<Claim>> GetClaimsAsync(string token)
	{
		var userToken = await database.UserTokens
			.Find(x => x.Id == token)
			.FirstOrDefaultAsync()
				?? throw new EntityNotFoundException();

		var userClaim = await database.UserClaims
			.Find(x => x.UserId == userToken.UserId)
			.FirstOrDefaultAsync()
				?? throw new DatabaseOperationErrorException();

		var claims = new List<Claim>
		{
			new(ClaimTypes.NameIdentifier, userToken.UserId),
			new(CustomClaimTypes.DateCreated, userToken.DateCreated.ToString("s")),
			new(CustomClaimTypes.DateExpired, userToken.DateExpired.ToString("s")),
		};
		foreach (var role in Enum.GetValues<Role>())
		{
			if (userClaim.Roles.HasFlag(role))
			{
				claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
			}
		}

		return claims;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="login"></param>
	/// <returns></returns>
	/// <exception cref="InvalidLoginException"></exception>
	/// <exception cref="DatabaseOperationErrorException"></exception>
	public async Task<TokenInfo> CreateTokenAsync(Login login)
	{
		var userLogin = await database.UserLogins
			.Find(x => x.Username == login.Username && x.Password == login.Password)
			.FirstOrDefaultAsync()
				?? throw new InvalidLoginException();

		var now = DateTime.UtcNow;
		var expiration = now.AddDays(14);
		var token = new UserToken
		{
			UserId = userLogin.UserId,
			DateCreated = now,
			DateExpired = expiration,
		};
		await database.UserTokens.DeleteManyAsync(x => x.UserId == userLogin.UserId);
		await database.UserTokens.InsertOneAsync(token);

		var userRoles = await database.UserClaims
			.Find(x => x.UserId == userLogin.UserId)
			.FirstOrDefaultAsync()
				?? throw new DatabaseOperationErrorException();

		return new TokenInfo(token.Id!, userLogin.UserId, userRoles.Roles, now, expiration);
	}

	public async Task DeleteTokenAsync(string token)
	{
		await database.UserTokens.DeleteOneAsync(x => x.Id == token);
	}

	public async Task DeleteTokenByUserIdAsync(string userId)
	{
		await database.UserTokens.DeleteManyAsync(x => x.UserId == userId);
	}
}
