using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MAUIApp.Services;
using MAUIApp.Services.HttpServices;

namespace MAUIApp.ViewModels;

public partial class SettingsViewModel(HttpService httpService) : ObservableObject
{
	public const string RouteName = "Settings";

	[ObservableProperty]
	private string _baseUri = httpService.BaseUri.AbsoluteUri;

	[ObservableProperty]
	private string _identityBaseUri = httpService.IdentityBaseUri.AbsoluteUri;


	[RelayCommand]
	private async Task SaveAsync()
	{
		httpService.BaseUri = new Uri(BaseUri);
		httpService.IdentityBaseUri = new Uri(IdentityBaseUri);
		await NavigationService.BackAsync();
	}
}
