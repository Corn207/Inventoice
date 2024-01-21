namespace MAUIApp.Services.HttpServices.Exceptions;
public class InvalidPasswordException(string? message = null) : HttpServiceException(message)
{
}
