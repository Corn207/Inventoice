﻿using Identity.Domain.DTOs;
using MAUIApp.Services.HttpServices.Exceptions;
using MAUIApp.Services.HttpServices.Interfaces;
using System.Net.Http.Json;

namespace MAUIApp.Services.HttpServices;
public sealed class IdentityService(HttpService httpService) : IIdentityService
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="body"></param>
	/// <returns></returns>
	/// <exception cref="ConnectionException"></exception>
	/// <exception cref="InvalidLoginException"></exception>
	/// <exception cref="ActionFailedException"></exception>
	public async Task LoginAsync(Login body)
	{
		await HttpService.ThrowIfNoConnection();

		var uri = httpService.IdentityBaseUri + "login";
		var response = await httpService.HttpClient.PostAsJsonAsync(uri, body);

		if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
			response.StatusCode == System.Net.HttpStatusCode.BadRequest)
		{
			throw new InvalidLoginException();
		}
		else if (!response.IsSuccessStatusCode)
		{
			var message =
				$"""
				{response.RequestMessage!.Method.Method} {response.StatusCode} {response.RequestMessage!.RequestUri!.PathAndQuery}
				{await response.Content.ReadAsStringAsync()}
				""";
			throw new ActionFailedException(message);
		}

		var tokenInfo = await HttpService.ReadContent<TokenInfo>(response);
		httpService.IdentityToken = tokenInfo.Token;
		httpService.IdentityTokenExpiry = tokenInfo.DateExpired;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	/// <exception cref="ConnectionException"></exception>
	/// <exception cref="InvalidLoginException"></exception>
	/// <exception cref="ActionFailedException"></exception>
	public async Task LogoutAsync()
	{
		await HttpService.ThrowIfNoConnection();

		var uri = httpService.IdentityBaseUri + "logout";
		var response = await httpService.HttpClient.PostAsync(uri, null);

		if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
		{
			throw new InvalidLoginException();
		}
		await HttpService.ThrowIfNotSuccessStatusCode(response);

		httpService.IdentityToken = string.Empty;
		httpService.IdentityTokenExpiry = DateTime.MinValue;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="body"></param>
	/// <returns></returns>
	/// <exception cref="ConnectionException"></exception>
	/// <exception cref="InvalidPasswordException"></exception>
	/// <exception cref="ActionFailedException"></exception>
	public async Task ChangePasswordAsync(
		ChangePassword body,
		CancellationToken cancellationToken = default)
	{
		await HttpService.ThrowIfNoConnection();

		var uri = httpService.IdentityBaseUri + "change-password";
		var response = await httpService.HttpClient.PatchAsJsonAsync(uri, body, cancellationToken);

		if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
		{
			await NavigationService.DisplayAlert("Sai mật khẩu", "Mật khẩu cũ không đúng", "OK");
			throw new InvalidPasswordException();
		}
		else
		{
			await HttpService.ThrowIfNotSuccessStatusCode(response);
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	/// <exception cref="ConnectionException"></exception>
	/// <exception cref="NotFoundException"></exception>
	/// <exception cref="ActionFailedException"></exception>
	public async Task ResetPasswordAsync(
		string id,
		CancellationToken cancellationToken = default)
	{
		await HttpService.ThrowIfNoConnection();

		var uri = httpService.IdentityBaseUri + $"{id}/reset-password";
		var response = await httpService.HttpClient.PatchAsync(uri, null, cancellationToken);

		if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
		{
			await NavigationService.DisplayAlert("Lỗi thao tác", "Không tìm thấy người dùng", "OK");
			throw new NotFoundException();
		}
		else
		{
			await HttpService.ThrowIfNotSuccessStatusCode(response);
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	/// <exception cref="ConnectionException"></exception>
	/// <exception cref="ActionFailedException"></exception>
	/// <exception cref="DeserializeException"></exception>
	public async Task<bool> CheckOnboardingAvailabilityAsync(
		CancellationToken cancellationToken = default)
	{
		await HttpService.ThrowIfNoConnection();

		var uri = httpService.IdentityBaseUri + "onboarding";
		HttpResponseMessage response;
		try
		{
			response = await httpService.HttpClient.GetAsync(uri, cancellationToken);
		}
		catch (HttpRequestException ex)
		{
			await NavigationService.DisplayAlert("Lỗi mạng", ex.Message, "OK");
			throw new ConnectionException();
		}

		await HttpService.ThrowIfNotSuccessStatusCode(response);

		var result = await HttpService.ReadContent<bool>(response, cancellationToken);
		return result;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="body"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	/// <exception cref="ConnectionException"></exception>
	/// <exception cref="ActionFailedException"></exception>
	public async Task OnboardingAsync(
		Onboarding body,
		CancellationToken cancellationToken = default)
	{
		await HttpService.ThrowIfNoConnection();

		var uri = httpService.IdentityBaseUri + "onboarding";
		var response = await httpService.HttpClient.PostAsJsonAsync(uri, body, cancellationToken);

		if (response.StatusCode == System.Net.HttpStatusCode.Gone)
		{
			await NavigationService.DisplayAlert("Lỗi thao tác", "Onboarding không khả dụng", "OK");
			throw new ActionFailedException();
		}
		else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
		{
			await NavigationService.DisplayAlert("Lỗi thao tác", "Đã tồn tại tên người dùng", "OK");
			throw new ActionFailedException();
		}
		else
		{
			await HttpService.ThrowIfNotSuccessStatusCode(response);
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	/// <exception cref="ConnectionException"></exception>
	/// <exception cref="ActionFailedException"></exception>
	public async Task<IdentityDetails> GetAsync(
		string id,
		CancellationToken cancellationToken = default)
	{
		await HttpService.ThrowIfNoConnection();

		var path = httpService.IdentityBaseUri + id;
		HttpResponseMessage response;
		try
		{
			response = await httpService.HttpClient.GetAsync(path, cancellationToken);
		}
		catch (HttpRequestException ex)
		{
			await NavigationService.DisplayAlert("Lỗi mạng", ex.Message, "OK");
			throw new ConnectionException();
		}

		await HttpService.ThrowIfNotSuccessStatusCode(response);
		var details = await HttpService.ReadContent<IdentityDetails>(response, cancellationToken);
		return details;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="body"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	/// <exception cref="ConnectionException"></exception>
	/// <exception cref="ActionFailedException"></exception>
	public async Task CreateAsync(
		CreateIdentity body,
		CancellationToken cancellationToken = default)
	{
		await HttpService.ThrowIfNoConnection();

		var path = httpService.IdentityBaseUri;
		HttpResponseMessage response;
		try
		{
			response = await httpService.HttpClient.PostAsJsonAsync(path, body, HttpService.JsonSerializerOptions, cancellationToken);
		}
		catch (HttpRequestException ex)
		{
			await NavigationService.DisplayAlert("Lỗi mạng", ex.Message, "OK");
			throw new ConnectionException();
		}

		await HttpService.ThrowIfNotSuccessStatusCode(response);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	/// <exception cref="ConnectionException"></exception>
	/// <exception cref="ActionFailedException"></exception>
	public async Task DeleteAsync(
		string id,
		CancellationToken cancellationToken = default)
	{
		await HttpService.ThrowIfNoConnection();

		var path = httpService.IdentityBaseUri + id;
		HttpResponseMessage response;
		try
		{
			response = await httpService.HttpClient.DeleteAsync(path, cancellationToken);
		}
		catch (HttpRequestException ex)
		{
			await NavigationService.DisplayAlert("Lỗi mạng", ex.Message, "OK");
			throw new ConnectionException();
		}

		await HttpService.ThrowIfNotSuccessStatusCode(response);
	}
}