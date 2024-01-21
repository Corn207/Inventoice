using Identity.Domain.DTOs;
using MAUIApp.Services.HttpServices.Exceptions;

namespace MAUIApp.Services.HttpServices.Interfaces;
public interface IIdentityService
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="body"></param>
	/// <returns></returns>
	/// <exception cref="ConnectionException"></exception>
	/// <exception cref="InvalidLoginException"></exception>
	/// <exception cref="ActionFailedException"></exception>
	Task LoginAsync(Login body);


	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	/// <exception cref="ConnectionException"></exception>
	/// <exception cref="InvalidLoginException"></exception>
	/// <exception cref="ActionFailedException"></exception>
	Task LogoutAsync();
}
