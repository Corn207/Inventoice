using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.Extensions.Options;
using WebAPI.Configurations;
using WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.Configure<MongoDBConnectionOptions>(
	builder.Configuration.GetSection(MongoDBConnectionOptions.SectionName));
builder.Services.AddSingleton(services =>
{
	var options = services.GetRequiredService<IOptions<MongoDBConnectionOptions>>();
	return new Database(options.Value.ConnectionString, options.Value.DatabaseName);
});
builder.Services.AddRepositories();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
