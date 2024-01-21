using MAUIApp.Services.HttpServices.Exceptions;
using MAUIApp.Utilities;
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

	public HttpClient HttpClient { get; } = new();
	#endregion


	#region HTTP pipeline helper methods
	public static async Task<ConnectionException> GetConnectionException(HttpRequestException exception)
	{
		await Shell.Current.DisplayAlert("Lỗi mạng", exception.Message, "OK");
		return new ConnectionException();
	}
	
	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	/// <exception cref="ConnectionException"></exception>
	public static async Task ThrowIfNoConnection()
	{
		if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
		{
			await NavigationService.DisplayAlert("Lỗi mạng", "Không có kết nối mạng.", "OK");
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
			await NavigationService.DisplayAlert("Lỗi thao tác", "Hết hạn quyền truy cập.", "OK");
			await NavigationService.ToLoginStackAsync();
			throw new ActionFailedException();
		}
		else if (response.StatusCode == HttpStatusCode.Forbidden)
		{
			await NavigationService.DisplayAlert("Lỗi thao tác", "Không có quyền thực hiện thao tác này.", "OK");
			throw new ActionFailedException();
		}
		else if (!response.IsSuccessStatusCode)
		{
			var message =
				$"""
				{response.RequestMessage!.Method.Method} {response.StatusCode} {response.RequestMessage!.RequestUri!.PathAndQuery}
				{await response.Content.ReadAsStringAsync()}
				""";
			await NavigationService.DisplayAlert("Lỗi mạng", message, "OK");
			throw new ActionFailedException();
		}
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
		var result = await response.Content.ReadFromJsonAsync<T>(cancellationToken);
		if (result is null)
		{
			await NavigationService.DisplayAlert("Lỗi dữ liệu", "Dữ liệu không khớp.", "OK");
			throw new DeserializeException();
		}
		return result;
	}

	public Uri GetUri(string relativeUri, IDictionary<string, object?>? queries = null)
	{
		if (string.IsNullOrWhiteSpace(relativeUri))
		{
			throw new ArgumentNullException(nameof(relativeUri));
		}

		if (queries is not null)
		{
			relativeUri += QueryStringConverter.Convert(queries);
		}
		return new Uri(BaseUri, relativeUri);
	}
	#endregion

	#region HTTP methods
	public async Task<T> GetAsync<T>(
		string path,
		IDictionary<string, object?>? queries = null,
		CancellationToken cancellationToken = default) where T : notnull
	{
		await ThrowIfNoConnection();

		var uri = GetUri(path, queries);
		HttpResponseMessage response;
		try
		{
			response = await HttpClient.GetAsync(uri, cancellationToken);
		}
		catch (HttpRequestException ex)
		{
			throw await GetConnectionException(ex);
		}

		await ThrowIfNotSuccessStatusCode(response);

		var result = await ReadContent<T>(response, cancellationToken);
		return result;
	}

	public async Task PostNoContentAsync<T>(
		string path,
		T body,
		CancellationToken cancellationToken = default) where T : notnull
	{
		await ThrowIfNoConnection();

		var uri = GetUri(path);
		HttpResponseMessage response;
		try
		{
			response = await HttpClient.PostAsJsonAsync(uri, body, JsonSerializerOptions, cancellationToken);
		}
		catch (HttpRequestException ex)
		{
			throw await GetConnectionException(ex);
		}

		await ThrowIfNotSuccessStatusCode(response);
	}

	public async Task<T> PostAsync<T>(
		string path,
		T body,
		CancellationToken cancellationToken = default) where T : notnull
	{
		await ThrowIfNoConnection();

		var uri = GetUri(path);
		HttpResponseMessage response;
		try
		{
			response = await HttpClient.PostAsJsonAsync(uri, body, JsonSerializerOptions, cancellationToken);
		}
		catch (HttpRequestException ex)
		{
			throw await GetConnectionException(ex);
		}

		await ThrowIfNotSuccessStatusCode(response);

		var result = await ReadContent<T>(response, cancellationToken);
		return result;
	}

	public async Task PutNoContentAsync<T>(
		string path,
		T body,
		CancellationToken cancellationToken = default) where T : notnull
	{
		await ThrowIfNoConnection();

		var uri = GetUri(path);
		HttpResponseMessage response;
		try
		{
			response = await HttpClient.PutAsJsonAsync(uri, body, JsonSerializerOptions, cancellationToken);
		}
		catch (HttpRequestException ex)
		{
			throw await GetConnectionException(ex);
		}

		await ThrowIfNotSuccessStatusCode(response);
	}

	public async Task<T> PutAsync<T>(
		string path,
		T body,
		CancellationToken cancellationToken = default) where T : notnull
	{
		await ThrowIfNoConnection();

		var uri = GetUri(path);
		HttpResponseMessage response;
		try
		{
			response = await HttpClient.PutAsJsonAsync(uri, body, JsonSerializerOptions, cancellationToken);
		}
		catch (HttpRequestException ex)
		{
			throw await GetConnectionException(ex);
		}

		await ThrowIfNotSuccessStatusCode(response);

		var result = await ReadContent<T>(response, cancellationToken);
		return result;
	}

	public async Task PatchNoContentAsync(
		string path,
		CancellationToken cancellationToken = default)
	{
		await ThrowIfNoConnection();

		var uri = GetUri(path);
		HttpResponseMessage response;
		try
		{
			response = await HttpClient.PatchAsync(uri, null, cancellationToken);
		}
		catch (HttpRequestException ex)
		{
			throw await GetConnectionException(ex);
		}

		await ThrowIfNotSuccessStatusCode(response);
	}

	public async Task PatchNoContentAsync<T>(
		string path,
		T body,
		CancellationToken cancellationToken = default) where T : notnull
	{
		await ThrowIfNoConnection();

		var uri = GetUri(path);
		HttpResponseMessage response;
		try
		{
			response = await HttpClient.PatchAsJsonAsync(uri, body, JsonSerializerOptions, cancellationToken);
		}
		catch (HttpRequestException ex)
		{
			throw await GetConnectionException(ex);
		}

		await ThrowIfNotSuccessStatusCode(response);
	}

	public async Task<T> PatchAsync<T>(
		string path,
		CancellationToken cancellationToken = default) where T : notnull
	{
		await ThrowIfNoConnection();

		var uri = GetUri(path);
		HttpResponseMessage response;
		try
		{
			response = await HttpClient.PatchAsync(uri, null, cancellationToken);
		}
		catch (HttpRequestException ex)
		{
			throw await GetConnectionException(ex);
		}

		await ThrowIfNotSuccessStatusCode(response);

		var result = await ReadContent<T>(response, cancellationToken);
		return result;
	}

	public async Task<T> PatchAsync<T>(
		string path,
		T body,
		CancellationToken cancellationToken = default) where T : notnull
	{
		await ThrowIfNoConnection();

		var uri = GetUri(path);
		HttpResponseMessage response;
		try
		{
			response = await HttpClient.PatchAsJsonAsync(uri, body, JsonSerializerOptions, cancellationToken);
		}
		catch (HttpRequestException ex)
		{
			throw await GetConnectionException(ex);
		}

		await ThrowIfNotSuccessStatusCode(response);

		var result = await ReadContent<T>(response, cancellationToken);
		return result;
	}

	public async Task DeleteAsync(
		string path,
		CancellationToken cancellationToken = default)
	{
		await ThrowIfNoConnection();

		var uri = GetUri(path);
		HttpResponseMessage response;
		try
		{
			response = await HttpClient.DeleteAsync(uri, cancellationToken);
		}
		catch (HttpRequestException ex)
		{
			throw await GetConnectionException(ex);
		}

		await ThrowIfNotSuccessStatusCode(response);
	}
	#endregion
}
