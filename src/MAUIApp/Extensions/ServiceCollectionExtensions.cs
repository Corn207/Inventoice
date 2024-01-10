using CommunityToolkit.Maui;
using MAUIApp.Pages;
using MAUIApp.Services;
using MAUIApp.Services.HttpServices;
using MAUIApp.Services.HttpServices.Interfaces;
using MAUIApp.ViewModels;

namespace MAUIApp.Extensions;
internal static class ServiceCollectionExtensions
{
	public static IServiceCollection AddServices(this IServiceCollection services)
	{
		services.AddSingleton<NavigationService>();
		services.AddSingleton<HttpService>();

		services.AddTransient<IAuditReportService, AuditReportService>();
		services.AddTransient<IClientService, ClientService>();
		services.AddTransient<IExportReportService, ExportReportService>();
		services.AddTransient<IImportReportService, ImportReportService>();
		services.AddTransient<IInvoiceService, InvoiceService>();
		services.AddTransient<IProductService, ProductService>();
		services.AddTransient<ICredentialService, CredentialService>();

		return services;
	}

	public static IServiceCollection AddViewsAndViewModels(this IServiceCollection services)
	{
		services.AddSingleton<AppShell>();
		services.AddTransientWithShellRoute<LoginPage, LoginViewModel>("login");

		services.AddTransientWithShellRoute<Pages.Invoices.ListPage, ViewModels.Invoices.ListViewModel>(ViewModels.Invoices.ListViewModel.RouteName);
		services.AddTransientWithShellRoute<Pages.Invoices.FilterPage, ViewModels.Invoices.FilterViewModel>(ViewModels.Invoices.FilterViewModel.RouteName);
		services.AddTransientWithShellRoute<Pages.Invoices.DetailsPage, ViewModels.Invoices.DetailsViewModel>(ViewModels.Invoices.DetailsViewModel.RouteName);
		services.AddTransientWithShellRoute<Pages.Invoices.CreatePage, ViewModels.Invoices.CreateViewModel>(ViewModels.Invoices.CreateViewModel.RouteName);
		services.AddTransientPopup<Pages.Invoices.EditItemPopup, ViewModels.Invoices.EditItemViewModel>();

		services.AddTransientWithShellRoute<Pages.Products.ListPage, ViewModels.Products.ListViewModel>(ViewModels.Products.ListViewModel.RouteName);
		services.AddTransientWithShellRoute<Pages.Products.FilterPage, ViewModels.Products.FilterViewModel>(ViewModels.Products.FilterViewModel.RouteName);
		services.AddTransientWithShellRoute<Pages.Products.DetailsPage, ViewModels.Products.DetailsViewModel>(ViewModels.Products.DetailsViewModel.RouteName);
		services.AddTransientWithShellRoute<Pages.Products.CreateUpdatePage, ViewModels.Products.CreateUpdateViewModel>(ViewModels.Products.CreateUpdateViewModel.RouteName);
		services.AddTransientWithShellRoute<Pages.Products.SearchPage, ViewModels.Products.SearchViewModel>(ViewModels.Products.SearchViewModel.RouteName);
		services.AddTransientWithShellRoute<Pages.Products.BarcodeScannerPage, ViewModels.Products.BarcodeScannerViewModel>(ViewModels.Products.BarcodeScannerViewModel.RouteName);

		services.AddSingleton<MorePage, MoreViewModel>();

		services.AddTransientWithShellRoute<Pages.ImportReports.ListPage, ViewModels.ImportReports.ListViewModel>(ViewModels.ImportReports.ListViewModel.RouteName);
		services.AddTransientWithShellRoute<Pages.ImportReports.FilterPage, ViewModels.ImportReports.FilterViewModel>(ViewModels.ImportReports.FilterViewModel.RouteName);
		services.AddTransientWithShellRoute<Pages.ImportReports.DetailsPage, ViewModels.ImportReports.DetailsViewModel>(ViewModels.ImportReports.DetailsViewModel.RouteName);
		services.AddTransientWithShellRoute<Pages.ImportReports.CreatePage, ViewModels.ImportReports.CreateViewModel>(ViewModels.ImportReports.CreateViewModel.RouteName);
		services.AddTransientPopup<Pages.ImportReports.EditItemPopup, ViewModels.ImportReports.EditItemViewModel>();

		services.AddTransientWithShellRoute<Pages.ExportReports.ListPage, ViewModels.ExportReports.ListViewModel>(ViewModels.ExportReports.ListViewModel.RouteName);
		services.AddTransientWithShellRoute<Pages.ExportReports.FilterPage, ViewModels.ExportReports.FilterViewModel>(ViewModels.ExportReports.FilterViewModel.RouteName);
		services.AddTransientWithShellRoute<Pages.ExportReports.DetailsPage, ViewModels.ExportReports.DetailsViewModel>(ViewModels.ExportReports.DetailsViewModel.RouteName);
		services.AddTransientWithShellRoute<Pages.ExportReports.CreatePage, ViewModels.ExportReports.CreateViewModel>(ViewModels.ExportReports.CreateViewModel.RouteName);
		services.AddTransientPopup<Pages.ExportReports.EditItemPopup, ViewModels.ExportReports.EditItemViewModel>();

		services.AddTransientWithShellRoute<Pages.AuditReports.ListPage, ViewModels.AuditReports.ListViewModel>(ViewModels.AuditReports.ListViewModel.RouteName);
		services.AddTransientWithShellRoute<Pages.AuditReports.FilterPage, ViewModels.AuditReports.FilterViewModel>(ViewModels.AuditReports.FilterViewModel.RouteName);
		services.AddTransientWithShellRoute<Pages.AuditReports.DetailsPage, ViewModels.AuditReports.DetailsViewModel>(ViewModels.AuditReports.DetailsViewModel.RouteName);
		services.AddTransientWithShellRoute<Pages.AuditReports.CreatePage, ViewModels.AuditReports.CreateViewModel>(ViewModels.AuditReports.CreateViewModel.RouteName);
		services.AddTransientPopup<Pages.AuditReports.EditItemPopup, ViewModels.AuditReports.EditItemViewModel>();

		services.AddTransientWithShellRoute<Pages.Clients.ListPage, ViewModels.Clients.ListViewModel>(ViewModels.Clients.ListViewModel.RouteName);
		services.AddTransientWithShellRoute<Pages.Clients.FilterPage, ViewModels.Clients.FilterViewModel>(ViewModels.Clients.FilterViewModel.RouteName);
		services.AddTransientWithShellRoute<Pages.Clients.DetailsPage, ViewModels.Clients.DetailsViewModel>(ViewModels.Clients.DetailsViewModel.RouteName);
		services.AddTransientWithShellRoute<Pages.Clients.CreateUpdatePage, ViewModels.Clients.CreateUpdateViewModel>(ViewModels.Clients.CreateUpdateViewModel.RouteName);
		services.AddTransientWithShellRoute<Pages.Clients.SearchPage, ViewModels.Clients.SearchViewModel>(ViewModels.Clients.SearchViewModel.RouteName);

		services.AddTransientWithShellRoute<Pages.DebugPage, ViewModels.DebugViewModel>(ViewModels.DebugViewModel.RouteName);

		return services;
	}
}
