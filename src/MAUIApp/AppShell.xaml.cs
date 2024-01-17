using MAUIApp.Services;
using MAUIApp.Services.HttpServices;

namespace MAUIApp;

public partial class AppShell : Shell
{
	public AppShell(HttpService httpService)
	{
		InitializeComponent();

		if (httpService.IdentityTokenExpiry <= DateTime.UtcNow)
		{
			GoToAsync($"//{NavigationService.LoginStackRouteName}");
		}
		else
		{
			GoToAsync($"//{NavigationService.MainStackRouteName}");
		}
	}
}
