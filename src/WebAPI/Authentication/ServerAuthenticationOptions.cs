using Microsoft.AspNetCore.Authentication;

namespace WebAPI.Authentication;

public class ServerAuthenticationOptions : AuthenticationSchemeOptions
{
	public const string DefaultScheme = "ServerAuthenticationScheme";
	public const string ValueHeader = "Bearer ";
}
