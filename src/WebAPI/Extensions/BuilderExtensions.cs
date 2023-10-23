using Infrastructure.Repositories;
using WebAPI.Configurations;

namespace WebAPI.Extensions;

public static class BuilderExtensions
{
	public static IServiceCollection AddRepositories(this IServiceCollection services)
	{
		services.AddScoped<ClientRepository>();
		services.AddScoped<ProductRepository>();

		return services;
	}
}
