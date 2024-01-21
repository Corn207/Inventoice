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
		var chars = new char[5];
		for (var i = 0; i < 5; i++)
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
	/// <exception cref="ArgumentException"></exception>
	/// <exception cref="EntityExistedException"></exception>
	public async Task<string> CreateIdentityAsync(string userId, CreateIdentity create, string? password = null)
	{
		if (create.Roles == 0)
		{
			throw new ArgumentException("Roles must be not empty");
		}

		var isExists = await database.UserLogins
			.Find(x => x.UserId == userId || x.Username == create.Username)
			.AnyAsync();
		if (isExists) throw new EntityExistedException();

		var userPassword = password ?? GenerateRandomPassword();
		var userLogin = new UserLogin()
		{
			UserId = userId,
			Username = create.Username,
			Password = userPassword,
		};
		var userClaim = new UserClaim()
		{
			UserId = userId,
			Roles = create.Roles,
		};
		await database.UserLogins.InsertOneAsync(userLogin);
		await database.UserClaims.InsertOneAsync(userClaim);

		return userPassword;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="userId"></param>
	/// <returns></returns>
	/// <exception cref="EntityNotFoundException"></exception>
	/// <exception cref="InvalidPasswordException"></exception>
	public async Task ChangePasswordAsync(string userId, ChangePassword body)
	{
		var entity = await database.UserLogins
			.Find(x => x.UserId == userId)
			.FirstOrDefaultAsync() ?? throw new EntityNotFoundException();
		if (entity.Password != body.OldPassword)
			throw new InvalidPasswordException();

		var updateDefinition = Builders<UserLogin>.Update.Set(x => x.Password, body.NewPassword);
		await database.UserLogins.UpdateOneAsync(x => x.UserId == userId, updateDefinition);
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
