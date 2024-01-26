using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MAUIApp.Services;
using MAUIApp.Services.HttpServices.Exceptions;
using MAUIApp.Services.HttpServices.Interfaces;

namespace MAUIApp.ViewModels;

public partial class MoreViewModel(
	IUserService userService,
	IIdentityService identityService) : ObservableObject
{
	public const string RouteName = "More";

	[ObservableProperty]
	private string? _nameOfUser;


	[RelayCommand]
	private async Task GetUserInfo()
	{
		try
		{
			var user = await userService.GetMeAsync();
			NameOfUser = user.Name;
		}
		catch (HttpServiceException)
		{
		}
	}

	[RelayCommand]
	private async Task LogoutAsync()
	{
		try
		{
			await identityService.LogoutAsync();
		}
		catch (HttpServiceException)
		{
		}

		await NavigationService.ToLoginStackAsync();
	}

	[RelayCommand]
	private static async Task GoToImportReportsPageAsync()
	{
		await NavigationService.ToAsync(ImportReports.ListViewModel.RouteName);
	}

	[RelayCommand]
	private static async Task GoToExportReportsPageAsync()
	{
		await NavigationService.ToAsync(ExportReports.ListViewModel.RouteName);
	}

	[RelayCommand]
	private static async Task GoToAuditReportsPageAsync()
	{
		await NavigationService.ToAsync(AuditReports.ListViewModel.RouteName);
	}

	[RelayCommand]
	private static async Task GoToClientsPageAsync()
	{
		await NavigationService.ToAsync(Clients.ListViewModel.RouteName);
	}

	[RelayCommand]
	private static async Task GoToDebugPageAsync()
	{
		await NavigationService.ToAsync(SettingsViewModel.RouteName);
	}

	[RelayCommand]
	private static async Task GoToUserListPageAsync()
	{
		await NavigationService.ToAsync(Users.ListViewModel.RouteName);
	}
}
