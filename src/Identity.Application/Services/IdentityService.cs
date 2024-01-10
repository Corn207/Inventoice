using Identity.Application.Exceptions;
using Identity.Domain.DTOs;
using Identity.Domain.Entity;
using MongoDB.Driver;

namespace Identity.Application.Services;
public class IdentityService(IdentityDatabase database)
{
	private const string _randomPasswordChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";


	private static string GenerateRandomPassword()
	{
		var random = new Random();
		var chars = new char[6];
		for (var i = 0; i < 6; i++)
		{
			var index = random.Next(0, _randomPasswordChars.Length);
			var c = _randomPasswordChars[index];
			chars[i] = c;
		}
		var password = new string(chars);
		return password;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="userId"></param>
	/// <returns></returns>
	/// <exception cref="EntityNotFoundException"></exception>
	/// <exception cref="DatabaseOperationErrorException"></exception>
	public async Task<IdentityDetails> GetIdentityAsync(string userId)
	{
		var userLogin = await database.UserLogins
			.Find(x => x.UserId == userId)
			.FirstOrDefaultAsync()
				?? throw new EntityNotFoundException();

		var userClaim = await database.UserClaims
			.Find(x => x.UserId == userId)
			.FirstOrDefaultAsync()
				?? throw new DatabaseOperationErrorException($"Claims not found for userId: {userId}");

		var result = new IdentityDetails(userId, userLogin.Username, userClaim.Roles);

		return result;
	}

	public async Task<bool> IsExistsIdentityAsync(string username)
	{
		var isExists = await database.UserLogins
			.Find(x => x.Username == username)
			.AnyAsync();

		return isExists;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="userId"></param>
	/// <param name="create"></param>
	/// <returns></returns>
	/// <exception cref="EntityExistedException"></exception>
	public async Task<string> CreateIdentityAsync(string userId, CreateIdentity create)
	{
		var isExists = await database.UserLogins
			.Find(x => x.UserId == userId || x.Username == create.Username)
			.AnyAsync();
		if (isExists) throw new EntityExistedException();

		var password = GenerateRandomPassword();
		var userLogin = new UserLogin()
		{
			UserId = userId,
			Username = create.Username,
			Password = password,
		};
		var userClaim = new UserClaim()
		{
			UserId = userId,
			Roles = create.Roles.Length > 0 ? create.Roles : [Role.Employee],
		};
		await database.UserLogins.InsertOneAsync(userLogin);
		await database.UserClaims.InsertOneAsync(userClaim);

		return password;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="userId"></param>
	/// <param name="create"></param>
	/// <returns></returns>
	/// <exception cref="EntityExistedException"></exception>
	public async Task CreateAdminIdentityAsync(string userId, Onboarding create)
	{
		var isExists = await database.UserLogins
			.Find(x => x.UserId == userId || x.Username == create.Username)
			.AnyAsync();
		if (isExists) throw new EntityExistedException();

		var userLogin = new UserLogin()
		{
			UserId = userId,
			Username = create.Username,
			Password = create.Password,
		};
		var userClaim = new UserClaim()
		{
			UserId = userId,
			Roles = [Role.Admin, Role.Manager, Role.Employee],
		};
		await database.UserLogins.InsertOneAsync(userLogin);
		await database.UserClaims.InsertOneAsync(userClaim);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="userId"></param>
	/// <returns></returns>
	/// <exception cref="EntityNotFoundException"></exception>
	public async Task<string> ResetPasswordAsync(string userId)
	{
		var newPassword = GenerateRandomPassword();

		var updateDefinition = Builders<UserLogin>.Update.Set(x => x.Password, newPassword);
		var result = await database.UserLogins.UpdateOneAsync(x => x.UserId == userId, updateDefinition);
		if (result.MatchedCount != 1)
			throw new EntityNotFoundException();

		return newPassword;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="userId"></param>
	/// <returns></returns>
	/// <exception cref="EntityNotFoundException"></exception>
	public async Task DeleteIdentityAsync(string userId)
	{
		var result = await database.UserLogins.DeleteOneAsync(x => x.UserId == userId);
		if (result.DeletedCount != 1)
			throw new EntityNotFoundException();

		await database.UserClaims.DeleteOneAsync(x => x.UserId == userId);
		await database.UserTokens.DeleteOneAsync(x => x.UserId == userId);
	}
}
