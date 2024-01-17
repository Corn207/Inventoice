using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebAPI.Exceptions;

namespace WebAPI.Extensions;

public static class ControllerExtensions
{
	public static string GetUserId(this ControllerBase controller)
	{
		var userId = controller.User.FindFirstValue(ClaimTypes.NameIdentifier);
		if (string.IsNullOrWhiteSpace(userId))
		{
			throw new InvalidClaimException();
		}

		return userId;
	}
}
