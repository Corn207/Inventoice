using MAUIApp.Pages;

namespace MAUIApp;

public partial class App : Application
{
	public App(AppShell appShell, LoginPage loginPage)
	{
		InitializeComponent();

		UserAppTheme = AppTheme.Light;
		MainPage = loginPage;
	}
}
