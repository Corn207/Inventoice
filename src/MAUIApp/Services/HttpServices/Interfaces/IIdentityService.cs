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

	/// <summary>
	/// 
	/// </summary>
	/// <param name="body"></param>
	/// <returns></returns>
	/// <exception cref="ConnectionException"></exception>
	/// <exception cref="InvalidPasswordException"></exception>
	/// <exception cref="ActionFailedException"></exception>
	Task ChangePasswordAsync(
		ChangePassword body,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	/// <exception cref="ConnectionException"></exception>
	/// <exception cref="NotFoundException"></exception>
	/// <exception cref="ActionFailedException"></exception>
	Task<string> ResetPasswordAsync(
		string id,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	/// <exception cref="ConnectionException"></exception>
	/// <exception cref="ActionFailedException"></exception>
	/// <exception cref="DeserializeException"></exception>
	Task<bool> CheckOnboardingAvailabilityAsync(
		CancellationToken cancellationToken = default);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="body"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	/// <exception cref="ConnectionException"></exception>
	/// <exception cref="ActionFailedException"></exception>
	Task OnboardingAsync(
		Onboarding body,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	/// <exception cref="ConnectionException"></exception>
	/// <exception cref="ActionFailedException"></exception>
	Task<IdentityDetails> GetAsync(
		string id,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="body"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	/// <exception cref="ConnectionException"></exception>
	/// <exception cref="ActionFailedException"></exception>
	Task<string> CreateAsync(
		CreateIdentity body,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	/// <exception cref="ConnectionException"></exception>
	/// <exception cref="ActionFailedException"></exception>
	Task DeleteAsync(
		string id,
		CancellationToken cancellationToken = default);
}
