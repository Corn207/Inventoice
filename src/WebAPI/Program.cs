using WebAPI.Configurations;
using WebAPI.Extensions;
using WebAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.Configure<MongoDBConnectionOptions>(builder.Configuration.GetSection(MongoDBConnectionOptions.SectionName));
builder.Services.AddDatabase();
builder.Services.AddRepositories();
builder.Services.AddEntityServices();
builder.Services.AddExceptionHandler<ExceptionHandler>();
builder.Services.AddCors(options =>
{
	options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();
app.UseAuthorization();
app.UseExceptionHandler(_ => { });

app.MapControllers();

app.Run();
