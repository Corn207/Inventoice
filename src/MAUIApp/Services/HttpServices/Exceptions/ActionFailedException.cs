namespace MAUIApp.Services.HttpServices.Exceptions;
public class ActionFailedException(string? message = null) : HttpServiceException(message)
{
}
