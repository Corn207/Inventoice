using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MAUIApp.Services;

namespace MAUIApp.ViewModels;

public partial class MoreViewModel : ObservableObject
{
	public const string RouteName = "More";

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
		await NavigationService.ToAsync(DebugViewModel.RouteName);
	}
}
