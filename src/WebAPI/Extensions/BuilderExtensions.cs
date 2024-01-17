using Application.Interfaces.Repositories;
using Application.Services;
using Identity.Application.Services;
using Infrastructure.Repositories;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using WebAPI.Configurations;

namespace WebAPI.Extensions;

public static class BuilderExtensions
{
	public static IHostApplicationBuilder AddConfigureOptions(this IHostApplicationBuilder builder)
	{
		builder.Services.Configure<MainMongoDBOptions>(builder.Configuration.GetSection(MainMongoDBOptions.SectionName));
		builder.Services.Configure<IdentityMongoDBOptions>(builder.Configuration.GetSection(IdentityMongoDBOptions.SectionName));

		return builder;
	}

	public static IServiceCollection AddMainPersistentServices(this IServiceCollection services)
	{
		services.AddSingleton(services =>
		{
			var options = services.GetRequiredService<IOptions<MainMongoDBOptions>>();
			var database = new MongoClient(options.Value.ConnectionString).GetDatabase(options.Value.DatabaseName);
			return new Infrastructure.Database(database);
		});

		services.AddScoped<IClientRepository, ClientRepository>();
		services.AddScoped<IProductRepository, ProductRepository>();
		services.AddScoped<IAuditReportRepository, AuditReportRepository>();
		services.AddScoped<IExportReportRepository, ExportReportRepository>();
		services.AddScoped<IImportReportRepository, ImportReportRepository>();
		services.AddScoped<IInvoiceRepository, InvoiceRepository>();
		services.AddScoped<IUserRepository, UserRepository>();

		services.AddScoped<ClientService>();
		services.AddScoped<ProductService>();
		services.AddScoped<AuditReportService>();
		services.AddScoped<ExportReportService>();
		services.AddScoped<ImportReportService>();
		services.AddScoped<InvoiceService>();
		services.AddScoped<UserService>();

		return services;
	}

	public static IServiceCollection AddIdentityServices(this IServiceCollection services)
	{
		services.AddSingleton(services =>
		{
			var options = services.GetRequiredService<IOptions<IdentityMongoDBOptions>>();
			var database = new MongoClient(options.Value.ConnectionString).GetDatabase(options.Value.DatabaseName);
			return new Identity.Application.IdentityDatabase(database);
		});
		services.AddScoped<IdentityService>();
		services.AddScoped<ClaimService>();
		services.AddScoped<TokenService>();

		return services;
	}

	public static IServiceCollection AddSwagger(this IServiceCollection services)
	{
		services.AddSwaggerGen(options =>
		{
			options.AddSecurityDefinition(
				"Bearer",
				new OpenApiSecurityScheme
				{
					Name = "Authorization",
					Description = "Authorization header using the Bearer scheme.",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.Http,
					Scheme = "Bearer",
				});
			options.AddSecurityRequirement(
				new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Id = "Bearer",
								Type = ReferenceType.SecurityScheme,
							}
						},
						Array.Empty<string>()
					}
				});
		});

		return services;
	}
}
