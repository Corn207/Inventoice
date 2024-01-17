using Identity.Domain.DTOs;
using MAUIApp.Services.HttpServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MAUIApp.Services.HttpServices;
public sealed class IdentityService(HttpService httpService) : IIdentityService
{
	//public async Task<TokenInfo> LoginAsync(Login body)
	//{
	//	await HttpService.ThrowIfNoConnection();

	//	var uri = HttpService.GetUri("login");
	//	HttpResponseMessage response;
	//	try
	//	{
	//		response = await _httpClient.PostAsJsonAsync(uri, body, _jsonSerializerOptions, cancellationToken);
	//	}
	//	catch (HttpRequestException ex)
	//	{
	//		throw await GetConnectionException(ex);
	//	}

	//	await ThrowIfNotSuccessStatusCode(response);

	//	var result = await ReadContent<T>(response, cancellationToken);
	//	return result;
	//}
}
