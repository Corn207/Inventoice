namespace Application.Exceptions;
public class InvalidIdException(string? message, string[] ids) : Exception(message)
{
	public InvalidIdException(string? message) : this(message, []) { }

	public string[] Ids { get; init; } = ids;
}
