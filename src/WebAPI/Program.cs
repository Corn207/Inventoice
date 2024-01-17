using WebAPI.Authentication;
using WebAPI.Extensions;
using WebAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.AddConfigureOptions();
builder.Services.AddMainPersistentServices();
builder.Services.AddIdentityServices();
builder.Services.AddAuthentication()
	.AddScheme<ServerAuthenticationOptions, ServerAuthenticationHandler>(ServerAuthenticationOptions.DefaultScheme, options => { });
builder.Services.AddExceptionHandler<ExceptionHandler>();
builder.Services.AddCors(options =>
{
	options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseExceptionHandler(_ => { });
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
