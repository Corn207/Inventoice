using Domain.Entities;
using MAUIApp.Services.HttpServices.Interfaces;

namespace MAUIApp.Services.HttpServices;
public class CredentialService : ICredentialService
{
	public User User { get; private set; } = new()
	{
		Id = "658b1b8a9811f8ef3ae5cdff",
		Name = "Quoc Anh",
		Phonenumber = "0986463215",
		DateCreated = DateTime.Now,
	};

	//public async Task Login(string username, string password)
	//{

	//}

	//public async Task Logout()
	//{

	//}

	//public async Task<User> GetAsync()
	//{

	//}


	//public async Task LoginAsync(string username, string password)
	//{
	//	if (!await CheckConnection())
	//	{
	//		throw new OperationCanceledException("No connection.");
	//	}

	//	var uri = new Uri($"login?username={username}&password={password}");
	//	HttpResponseMessage response;
	//	try
	//	{
	//		response = await HttpClient.GetAsync(uri);
	//	}
	//	catch (HttpRequestException ex)
	//	{
	//		await Shell.Current.DisplayAlert("Lỗi mạng", ex.Message, "OK");
	//		throw new OperationCanceledException("No connection.");
	//	}

	//	if (!response.IsSuccessStatusCode)
	//	{
	//		var errorMessage = await DisplayErrorMessage(response);
	//		throw new OperationCanceledException(errorMessage);
	//	}

	//	var token = await response.Content.ReadAsStringAsync();

	//	HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
	//	UserId = token;
	//}

}
