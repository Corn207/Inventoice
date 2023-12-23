namespace Application.Exceptions;
public class InvalidIdException(string? message, params string[] ids) : Exception(message)
{
	public string[] Ids { get; init; } = ids;
}
