namespace Application.Exceptions;
public class InvalidIdException : Exception
{
	public InvalidIdException(string[] ids)
	{
		Ids = ids;
	}

	public string[] Ids { get; }
}
