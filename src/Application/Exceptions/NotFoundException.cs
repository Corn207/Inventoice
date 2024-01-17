namespace Application.Exceptions;
public class NotFoundException(string? message = null, params string[] ids) : Exception(message)
{
	public string[] Ids { get; } = ids;
}
