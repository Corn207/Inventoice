using CommunityToolkit.Mvvm.Input;
using MAUIApp.Services;

namespace MAUIApp.ViewModels;
public static class StaticViewModel
{
	private static AsyncRelayCommand? _closePageCommand;
	public static IAsyncRelayCommand ClosePageCommand => _closePageCommand ??= new AsyncRelayCommand(ClosePageAsync);
	private static async Task ClosePageAsync()
	{
		await NavigationService.BackAsync();
	}

	private static AsyncRelayCommand<string>? _toProductDetailsPageCommand;
	public static IAsyncRelayCommand<string> ToProductDetailsPageCommand => _toProductDetailsPageCommand ??= new AsyncRelayCommand<string>(ToProductDetailsPageAsync);
	private static async Task ToProductDetailsPageAsync(string? id)
	{
		if (id is null) return;
		await NavigationService.ToAsync(Products.DetailsViewModel.RouteName, Products.DetailsViewModel.QueryId, id);
	}

	private static AsyncRelayCommand<string>? _toClientDetailsPageCommand;
	public static IAsyncRelayCommand<string> ToClientDetailsPageCommand => _toClientDetailsPageCommand ??= new AsyncRelayCommand<string>(ToClientDetailsPageAsync);
	private static async Task ToClientDetailsPageAsync(string? id)
	{
		if (id is null) return;
		await NavigationService.ToAsync(Clients.DetailsViewModel.RouteName, Clients.DetailsViewModel.QueryId, id);
	}

	private static AsyncRelayCommand<string>? _toInvoiceDetailsPageCommand;
	public static IAsyncRelayCommand<string> ToInvoiceDetailsPageCommand => _toInvoiceDetailsPageCommand ??= new AsyncRelayCommand<string>(ToInvoiceDetailsPageAsync);
	private static async Task ToInvoiceDetailsPageAsync(string? id)
	{
		if (id is null) return;
		await NavigationService.ToAsync(Invoices.DetailsViewModel.RouteName, Invoices.DetailsViewModel.QueryId, id);
	}

	private static AsyncRelayCommand<string>? _toImportReportDetailsPageCommand;
	public static IAsyncRelayCommand<string> ToImportReportDetailsPageCommand => _toImportReportDetailsPageCommand ??= new AsyncRelayCommand<string>(ToImportReportDetailsPageAsync);
	private static async Task ToImportReportDetailsPageAsync(string? id)
	{
		if (id is null) return;
		await NavigationService.ToAsync(ImportReports.DetailsViewModel.RouteName, ImportReports.DetailsViewModel.QueryId, id);
	}

	private static AsyncRelayCommand<string>? _toExportReportDetailsPageCommand;
	public static IAsyncRelayCommand<string> ToExportReportDetailsPageCommand => _toExportReportDetailsPageCommand ??= new AsyncRelayCommand<string>(ToExportReportDetailsPageAsync);
	private static async Task ToExportReportDetailsPageAsync(string? id)
	{
		if (id is null) return;
		await NavigationService.ToAsync(ExportReports.DetailsViewModel.RouteName, ExportReports.DetailsViewModel.QueryId, id);
	}

	private static AsyncRelayCommand<string>? _toAuditReportDetailsPageCommand;
	public static IAsyncRelayCommand<string> ToAuditReportDetailsPageCommand => _toAuditReportDetailsPageCommand ??= new AsyncRelayCommand<string>(ToAuditReportDetailsPageAsync);
	private static async Task ToAuditReportDetailsPageAsync(string? id)
	{
		if (id is null) return;
		await NavigationService.ToAsync(AuditReports.DetailsViewModel.RouteName, AuditReports.DetailsViewModel.QueryId, id);
	}

	private static AsyncRelayCommand<string>? _toBarcodeScannerPageCommand;
	public static IAsyncRelayCommand<string> ToBarcodeScannerPageCommand => _toBarcodeScannerPageCommand ??= new AsyncRelayCommand<string>(ToBarcodeScannerPageAsync);
	private static async Task ToBarcodeScannerPageAsync(string? returnId)
	{
		if (returnId is null) return;
		await NavigationService.ToAsync(Products.BarcodeScannerViewModel.RouteName, Products.BarcodeScannerViewModel.QueryReturnId, returnId);
	}

	private static AsyncRelayCommand<string>? _toProductSearchPageCommand;
	public static IAsyncRelayCommand<string> ToProductSearchPageCommand => _toProductSearchPageCommand ??= new AsyncRelayCommand<string>(ToProductSearchPageAsync);
	private static async Task ToProductSearchPageAsync(string? returnId)
	{
		if (returnId is null) return;
		await NavigationService.ToAsync(Products.SearchViewModel.RouteName, Products.SearchViewModel.QueryReturnId, returnId);
	}

	private static AsyncRelayCommand<string>? _toClientSearchPageCommand;
	public static IAsyncRelayCommand<string> ToClientSearchPageCommand => _toClientSearchPageCommand ??= new AsyncRelayCommand<string>(ToClientSearchPageAsync);
	private static async Task ToClientSearchPageAsync(string? returnId)
	{
		if (returnId is null) return;
		await NavigationService.ToAsync(Clients.SearchViewModel.RouteName, Clients.SearchViewModel.QueryReturnId, returnId);
	}

	private static AsyncRelayCommand<string>? _toUserDetailsAdminPageCommand;
	public static IAsyncRelayCommand<string> ToUserDetailsAdminPageCommand => _toUserDetailsAdminPageCommand ??= new AsyncRelayCommand<string>(ToUserDetailsAdminPageAsync);
	private static async Task ToUserDetailsAdminPageAsync(string? id)
	{
		if (id is null) return;
		await NavigationService.ToAsync(Users.DetailsAdminViewModel.RouteName, Users.DetailsAdminViewModel.QueryId, id);
	}
}
