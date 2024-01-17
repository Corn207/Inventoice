using Identity.Domain.DTOs;
using Identity.Domain.Entity;
using Identity.Application.Exceptions;
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
	/// <exception cref="InvalidTokenException"></exception>
	public async Task<List<Claim>> GetClaimsAsync(string token)
	{
		var userToken = await database.UserTokens
			.Find(x => x.Id == token)
			.FirstOrDefaultAsync()
				?? throw new EntityNotFoundException();

		var userClaim = await database.UserClaims
			.Find(x => x.UserId == userToken.UserId)
			.FirstOrDefaultAsync()
				?? throw new EntityNotFoundException();

		var claims = new List<Claim>
		{
			new(ClaimTypes.NameIdentifier, userToken.UserId),
			new(CustomClaimTypes.DateCreated, userToken.DateCreated.ToString("s")),
			new(CustomClaimTypes.DateExpired, userToken.DateExpired.ToString("s")),
		};
		foreach (var role in userClaim.Roles)
		{
			claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
		}

		return claims;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="username"></param>
	/// <param name="password"></param>
	/// <returns></returns>
	/// <exception cref="InvalidLoginException"></exception>
	public async Task<TokenInfo> CreateTokenAsync(string username, string password)
	{
		var userLogin = await database.UserLogins
			.Find(x => x.Username == username && x.Password == password)
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

		return new TokenInfo(token.Id!, userLogin.UserId, now, expiration);
	}

	public async Task RevokeTokenAsync(string token)
	{
		await database.UserTokens.DeleteOneAsync(x => x.Id == token);
	}

	public async Task RevokeTokenByUserIdAsync(string userId)
	{
		await database.UserTokens.DeleteManyAsync(x => x.UserId == userId);
	}
}
