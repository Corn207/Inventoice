using Identity.Application;
using Identity.Application.Exceptions;
using Identity.Application.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace WebAPI.Authentication;

public class ServerAuthenticationHandler(
	IOptionsMonitor<ServerAuthenticationOptions> options,
	ILoggerFactory logger,
	UrlEncoder encoder,
	TokenService tokenService)
	: AuthenticationHandler<ServerAuthenticationOptions>(options, logger, encoder)
{
	protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
	{
		if (!Request.Headers.TryGetValue("Authorization", out var value))
			return AuthenticateResult.Fail("Missing Authorization Header.");

		if (value.Count != 1)
			return AuthenticateResult.Fail("Invalid Authorization Header Count.");

		var token = value[0];
		if (string.IsNullOrWhiteSpace(token) || !token.StartsWith(ServerAuthenticationOptions.ValueHeader, StringComparison.OrdinalIgnoreCase))
			return AuthenticateResult.Fail("Invalid Authorization Header");

		token = token[ServerAuthenticationOptions.ValueHeader.Length..].Trim();

		List<Claim> claims;
		try
		{
			claims = await tokenService.GetClaimsAsync(token);
		}
		catch (EntityNotFoundException)
		{
			return AuthenticateResult.Fail("Token not found or claims not found.");
		}

		var dateExpiredString = claims.First(x => x.Type == CustomClaimTypes.DateExpired).Value;
		var dateExpired = DateTime.Parse(dateExpiredString, CultureInfo.InvariantCulture);
		if (DateTime.UtcNow >= dateExpired)
		{
			await tokenService.DeleteTokenAsync(token);
			return AuthenticateResult.Fail("Token expired.");
		}

		var identity = new ClaimsIdentity(claims, Scheme.Name);
		var principal = new ClaimsPrincipal(identity);
		var ticket = new AuthenticationTicket(principal, Scheme.Name);

		return AuthenticateResult.Success(ticket);
	}
}
