using Identity.Domain.Entity;
using MAUIApp.Services.HttpServices.Exceptions;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace MAUIApp.Services.HttpServices;
public sealed class HttpService
{
	public HttpService()
	{
		IdentityToken = Preferences.Get(_identityTokenKey, string.Empty);
	}

	#region HttpClient configurations
	public static readonly JsonSerializerOptions JsonSerializerOptions = new()
	{
		PropertyNameCaseInsensitive = true,
	};

	private const string _baseUriKey = "BaseUri";
	private Uri _baseUri = new(Preferences.Get(_baseUriKey, "http://10.0.2.2:21080/api/"));
	public Uri BaseUri
	{
		get => _baseUri;
		set
		{
			_baseUri = value;
			Preferences.Set(_baseUriKey, value.AbsoluteUri);
		}
	}

	private const string _identityBaseUriKey = "IdentityBaseUri";
	private Uri _identityBaseUri = new(Preferences.Get(_identityBaseUriKey, "http://10.0.2.2:21080/api/Identities/"));
	public Uri IdentityBaseUri
	{
		get => _identityBaseUri;
		set
		{
			_identityBaseUri = value;
			Preferences.Set(_identityBaseUriKey, value.AbsoluteUri);
		}
	}

	private const string _identityTokenKey = "IdentityToken";
	public string IdentityToken
	{
		set
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				Preferences.Remove(_identityTokenKey);
				HttpClient.DefaultRequestHeaders.Authorization = null;
			}
			else
			{
				Preferences.Set(_identityTokenKey, value);
				HttpClient.DefaultRequestHeaders.Authorization = new("Bearer", value);
			}
		}
	}

	private const string _identityTokenExpiryKey = "IdentityTokenExpiry";
	private DateTime _identityTokenExpiry = Preferences.Get(_identityTokenExpiryKey, DateTime.MinValue);
	public DateTime IdentityTokenExpiry
	{
		get => _identityTokenExpiry;
		set
		{
			_identityTokenExpiry = value;
			Preferences.Set(_identityTokenExpiryKey, value);
		}
	}

	private const string _identityRolesKey = "IdentityRoles";
	private Role _identityRoles = (Role)Preferences.Get(_identityRolesKey, 0);
	public Role IdentityRoles
	{
		get => _identityRoles;
		set
		{
			_identityRoles = value;
			Preferences.Set(_identityRolesKey, (int)value);
		}
	}

	public HttpClient HttpClient { get; } = new()
	{
		Timeout = TimeSpan.FromSeconds(10),
	};
	#endregion

	#region HTTP pipeline helper methods
	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	/// <exception cref="ConnectionException"></exception>
	public static async Task ThrowIfNoConnection()
	{
		if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
		{
			await NavigationService.DisplayAlertAsync("Lỗi mạng", "Không có kết nối mạng.", "OK");
			throw new ConnectionException();
		}
	}

	/// <summary>
	/// Throw <see cref="ActionFailedException"/> if <paramref name="response"/> is not success status code.
	/// </summary>
	/// <param name="response"></param>
	/// <returns></returns>
	/// <exception cref="ActionFailedException"></exception>
	public static async Task ThrowIfNotSuccessStatusCode(HttpResponseMessage response)
	{
		if (response.StatusCode == HttpStatusCode.Unauthorized)
		{
			await NavigationService.DisplayAlertAsync("Lỗi thao tác", "Hết hạn quyền truy cập.", "OK");
			await NavigationService.ToLoginStackAsync();
			throw new ActionFailedException();
		}
		else if (response.StatusCode == HttpStatusCode.Forbidden)
		{
			await NavigationService.DisplayAlertAsync("Lỗi thao tác", "Không có quyền thực hiện thao tác này.", "OK");
			throw new ActionFailedException();
		}
		else if (!response.IsSuccessStatusCode)
		{
			var message =
				$"""
				{response.RequestMessage!.Method.Method} {response.StatusCode} {response.RequestMessage!.RequestUri!.PathAndQuery}
				{await response.Content.ReadAsStringAsync()}
				""";
			await NavigationService.DisplayAlertAsync("Lỗi mạng", message, "OK");
			throw new ActionFailedException();
		}
	}

	public static async Task HandleTimeoutAsync()
	{
		await NavigationService.DisplayAlertAsync("Lỗi mạng", "Quá thời gian gửi yêu cầu.", "OK");
	}

	/// <summary>
	/// Throw <see cref="DeserializeException"/> if <paramref name="response"/> content is null.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="response"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	/// <exception cref="DeserializeException"></exception>
	public static async Task<T> ReadContent<T>(
		HttpResponseMessage response,
		CancellationToken cancellationToken = default) where T : notnull
	{
		try
		{
			var result = await response.Content.ReadFromJsonAsync<T>(cancellationToken);
			return result is null ? throw new DeserializeException() : result;
		}
		catch (Exception ex) when (
			ex is JsonException ||
			ex is NotSupportedException ||
			ex is ArgumentNullException || 
			ex is DeserializeException)
		{
			await NavigationService.DisplayAlertAsync("Lỗi dữ liệu", "Dữ liệu không khớp.", "OK");
			throw new DeserializeException();
		}
	}
	#endregion

	#region HTTP methods
	public async Task<T> GetAsync<T>(
		Uri uri,
		CancellationToken cancellationToken = default) where T : notnull
	{
		await ThrowIfNoConnection();

		try
		{
			using var response = await HttpClient.GetAsync(uri, cancellationToken);
			await ThrowIfNotSuccessStatusCode(response);

			var result = await ReadContent<T>(response, cancellationToken);
			return result;
		}
		catch (TaskCanceledException ex) when (ex.InnerException is System.TimeoutException)
		{
			await HandleTimeoutAsync();
			throw new HttpServiceException();
		}
	}

	public async Task PostNoContentAsync<T>(
		Uri uri,
		T body,
		CancellationToken cancellationToken = default) where T : notnull
	{
		await ThrowIfNoConnection();

		try
		{
			using var response = await HttpClient.PostAsJsonAsync(uri, body, JsonSerializerOptions, cancellationToken);
			await ThrowIfNotSuccessStatusCode(response);
		}
		catch (TaskCanceledException ex) when (ex.InnerException is System.TimeoutException)
		{
			await HandleTimeoutAsync();
			throw new HttpServiceException();
		}
	}

	public async Task<T> PostAsync<T>(
		Uri uri,
		T body,
		CancellationToken cancellationToken = default) where T : notnull
	{
		await ThrowIfNoConnection();

		try
		{
			using var response = await HttpClient.PostAsJsonAsync(uri, body, JsonSerializerOptions, cancellationToken);
			await ThrowIfNotSuccessStatusCode(response);

			var result = await ReadContent<T>(response, cancellationToken);
			return result;
		}
		catch (TaskCanceledException ex) when (ex.InnerException is System.TimeoutException)
		{
			await HandleTimeoutAsync();
			throw new HttpServiceException();
		}
	}

	public async Task PutNoContentAsync<T>(
		Uri uri,
		T body,
		CancellationToken cancellationToken = default) where T : notnull
	{
		await ThrowIfNoConnection();

		try
		{
			using var response = await HttpClient.PutAsJsonAsync(uri, body, JsonSerializerOptions, cancellationToken);
			await ThrowIfNotSuccessStatusCode(response);
		}
		catch (TaskCanceledException ex) when (ex.InnerException is System.TimeoutException)
		{
			await HandleTimeoutAsync();
			throw new HttpServiceException();
		}
	}

	public async Task<T> PutAsync<T>(
		Uri uri,
		T body,
		CancellationToken cancellationToken = default) where T : notnull
	{
		await ThrowIfNoConnection();

		try
		{
			using var response = await HttpClient.PutAsJsonAsync(uri, body, JsonSerializerOptions, cancellationToken);
			await ThrowIfNotSuccessStatusCode(response);

			var result = await ReadContent<T>(response, cancellationToken);
			return result;
		}
		catch (TaskCanceledException ex) when (ex.InnerException is System.TimeoutException)
		{
			await HandleTimeoutAsync();
			throw new HttpServiceException();
		}
	}

	public async Task PatchNoContentAsync(
		Uri uri,
		CancellationToken cancellationToken = default)
	{
		await ThrowIfNoConnection();

		try
		{
			using var response = await HttpClient.PatchAsync(uri, null, cancellationToken);
			await ThrowIfNotSuccessStatusCode(response);
		}
		catch (TaskCanceledException ex) when (ex.InnerException is System.TimeoutException)
		{
			await HandleTimeoutAsync();
			throw new HttpServiceException();
		}
	}

	public async Task PatchNoContentAsync<T>(
		Uri uri,
		T body,
		CancellationToken cancellationToken = default) where T : notnull
	{
		await ThrowIfNoConnection();

		try
		{
			using var response = await HttpClient.PatchAsJsonAsync(uri, body, JsonSerializerOptions, cancellationToken);
			await ThrowIfNotSuccessStatusCode(response);
		}
		catch (TaskCanceledException ex) when (ex.InnerException is System.TimeoutException)
		{
			await HandleTimeoutAsync();
			throw new HttpServiceException();
		}
	}

	public async Task<T> PatchAsync<T>(
		Uri uri,
		CancellationToken cancellationToken = default) where T : notnull
	{
		await ThrowIfNoConnection();

		try
		{
			using var response = await HttpClient.PatchAsync(uri, null, cancellationToken);
			await ThrowIfNotSuccessStatusCode(response);

			var result = await ReadContent<T>(response, cancellationToken);
			return result;
		}
		catch (TaskCanceledException ex) when (ex.InnerException is System.TimeoutException)
		{
			await HandleTimeoutAsync();
			throw new HttpServiceException();
		}
	}

	public async Task<T> PatchAsync<T>(
		Uri uri,
		T body,
		CancellationToken cancellationToken = default) where T : notnull
	{
		await ThrowIfNoConnection();

		try
		{
			using var response = await HttpClient.PatchAsJsonAsync(uri, body, JsonSerializerOptions, cancellationToken);
			await ThrowIfNotSuccessStatusCode(response);

			var result = await ReadContent<T>(response, cancellationToken);
			return result;
		}
		catch (TaskCanceledException ex) when (ex.InnerException is System.TimeoutException)
		{
			await HandleTimeoutAsync();
			throw new HttpServiceException();
		}
	}

	public async Task DeleteAsync(
		Uri uri,
		CancellationToken cancellationToken = default)
	{
		await ThrowIfNoConnection();

		try
		{
			using var response = await HttpClient.DeleteAsync(uri, cancellationToken);
			await ThrowIfNotSuccessStatusCode(response);
		}
		catch (TaskCanceledException ex) when (ex.InnerException is System.TimeoutException)
		{
			await HandleTimeoutAsync();
			throw new HttpServiceException();
		}
	}
	#endregion
}
