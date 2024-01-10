using MAUIApp.Services.HttpServices.Exceptions;
using MAUIApp.Utilities;
using System.Net.Http.Json;
using System.Text.Json;

namespace MAUIApp.Services.HttpServices;
public sealed class HttpService
{
	#region HttpClient configurations
	private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
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

	public HttpClient HttpClient { get; } = new();
	#endregion


	#region HTTP pipeline helper methods
	public static async Task ThrowIfNoConnection()
	{
		if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
		{
			await Shell.Current.DisplayAlert("Lỗi mạng", "Không có kết nối mạng.", "OK");
			throw new ConnectionException();
		}
	}

	public static async Task<ConnectionException> GetConnectionException(HttpRequestException exception)
	{
		await Shell.Current.DisplayAlert("Lỗi mạng", exception.Message, "OK");
		return new ConnectionException();
	}

	public static async Task ThrowIfNotSuccessStatusCode(HttpResponseMessage response)
	{
		if (!response.IsSuccessStatusCode)
		{
			var message =
				$"""
				{response.RequestMessage!.Method.Method} {response.StatusCode} {response.RequestMessage!.RequestUri!.PathAndQuery}
				{await response.Content.ReadAsStringAsync()}
				""";
			await Shell.Current.DisplayAlert("Lỗi dữ liệu", message, "OK");
			throw new ActionFailedException();
		}
	}

	public static async Task<T> ReadContent<T>(
		HttpResponseMessage response,
		CancellationToken cancellationToken = default)
	{
		var result = await response.Content.ReadFromJsonAsync<T>(cancellationToken);
		if (result is null)
		{
			await Shell.Current.DisplayAlert("Lỗi dữ liệu", "Dữ liệu không khớp.", "OK");
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
		CancellationToken cancellationToken = default)
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
			response = await HttpClient.PostAsJsonAsync(uri, body, _jsonSerializerOptions, cancellationToken);
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
			response = await HttpClient.PostAsJsonAsync(uri, body, _jsonSerializerOptions, cancellationToken);
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
			response = await HttpClient.PutAsJsonAsync(uri, body, _jsonSerializerOptions, cancellationToken);
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
			response = await HttpClient.PutAsJsonAsync(uri, body, _jsonSerializerOptions, cancellationToken);
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
			response = await HttpClient.PatchAsJsonAsync(uri, body, _jsonSerializerOptions, cancellationToken);
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
			response = await HttpClient.PatchAsJsonAsync(uri, body, _jsonSerializerOptions, cancellationToken);
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
