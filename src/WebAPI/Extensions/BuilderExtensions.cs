using Application.Interfaces.Repositories;
using Application.Services;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WebAPI.Configurations;

namespace WebAPI.Extensions;

public static class BuilderExtensions
{
	public static IServiceCollection AddDatabase(this IServiceCollection services)
	{
		services.AddSingleton(services =>
		{
			var options = services.GetRequiredService<IOptions<MongoDBConnectionOptions>>();
			return new MongoClient(options.Value.ConnectionString).GetDatabase(options.Value.DatabaseName);
		});
		services.AddSingleton<Database>();

		return services;
	}

	public static IServiceCollection AddRepositories(this IServiceCollection services)
	{
		services.AddScoped<IClientRepository, ClientRepository>();
		services.AddScoped<IProductRepository, ProductRepository>();
		services.AddScoped<IAuditReportRepository, AuditReportRepository>();
		services.AddScoped<IExportReportRepository, ExportReportRepository>();
		services.AddScoped<IImportReportRepository, ImportReportRepository>();
		services.AddScoped<IInvoiceRepository, InvoiceRepository>();

		return services;
	}

	public static IServiceCollection AddEntityServices(this IServiceCollection services)
	{
		services.AddScoped<ClientService>();
		services.AddScoped<ProductService>();
		services.AddScoped<AuditReportService>();
		services.AddScoped<ExportReportService>();
		services.AddScoped<ImportReportService>();
		services.AddScoped<InvoiceService>();

		return services;
	}
}
