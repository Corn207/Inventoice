namespace Identity.Application.Exceptions;
public class DatabaseOperationErrorException(string? message = null) : Exception(message)
{
}
