using Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Exceptions;

namespace WebAPI.Middlewares;

public class ExceptionHandler : IExceptionHandler
{
	public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
	{
		var details = new ProblemDetails()
		{
			Status = httpContext.Response.StatusCode,
			Type = "https://tools.ietf.org/html/rfc9110",
			Title = "An error occurred",
			Detail = exception.Message,
			Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
		};
		switch (exception)
		{
			case InvalidIdException:
				details.Status = StatusCodes.Status404NotFound;
				details.Detail += "\nIds: " + string.Join(", ", ((InvalidIdException)exception).Ids);
				break;
			case OutOfStockException:
				details.Status = StatusCodes.Status400BadRequest;
				details.Detail += "\nIds: " + string.Join(", ", ((OutOfStockException)exception).Ids);
				break;
			case InvalidClaimException:
			case UnknownException:
				details.Status = StatusCodes.Status500InternalServerError;
				break;
			case FormatException:
				details.Status = StatusCodes.Status400BadRequest;
				break;
			default:
				return false;
		};

		await httpContext.Response.WriteAsJsonAsync(details, cancellationToken);

		return true;
	}
}
