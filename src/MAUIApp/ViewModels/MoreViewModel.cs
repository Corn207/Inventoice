using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Identity.Domain.Entity;
using MAUIApp.Services;
using MAUIApp.Services.HttpServices;
using MAUIApp.Services.HttpServices.Exceptions;
using MAUIApp.Services.HttpServices.Interfaces;

namespace MAUIApp.ViewModels;
public partial class MoreViewModel(
	HttpService httpService,
	IUserService userService,
	IIdentityService identityService) : ObservableObject
{
	public const string RouteName = "More";

	[ObservableProperty]
	private string? _nameOfUser;

	[ObservableProperty]
	private string? _roleOfUser;

	[ObservableProperty]
	private bool _isManager = false;

	[ObservableProperty]
	private bool _isAdmin = false;


	[RelayCommand]
	private async Task LoadedAsync()
	{
		try
		{
			var user = await userService.GetMeAsync();
			NameOfUser = user.Name;

			var highestRole = Enum.GetValues<Role>().First(r => httpService.IdentityRoles.HasFlag(r));
			RoleOfUser = highestRole switch
			{
				Role.Admin => "Quản trị viên",
				Role.Manager => "Quản lý",
				Role.Employee => "Nhân viên",
				_ => null,
			};
			IsManager = httpService.IdentityRoles.HasFlag(Role.Manager);
			IsAdmin = httpService.IdentityRoles.HasFlag(Role.Admin);
		}
		catch (HttpServiceException)
		{
			return;
		}
	}

	[RelayCommand]
	private static async Task ToUserDetailsUserPage()
	{
		await NavigationService.ToAsync(Users.DetailsUserViewModel.RouteName);
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
