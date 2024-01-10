using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MAUIApp.Services;
using MAUIApp.Services.HttpServices;

namespace MAUIApp.ViewModels;

public partial class DebugViewModel(HttpService httpClientService) : ObservableObject
{
	public const string RouteName = "Debug";

	[ObservableProperty]
	private string _baseUrl = httpClientService.BaseUri.AbsoluteUri;

	[RelayCommand]
	private static async Task ClosePageAsync()
	{
		await NavigationService.BackAsync();
	}

	[RelayCommand]
	private async Task SaveAndCloseAsync()
	{
		// TODO: Rename from BaseUrl to BaseUri
		httpClientService.BaseUri = new Uri(BaseUrl);
		await NavigationService.BackAsync();
	}
}
