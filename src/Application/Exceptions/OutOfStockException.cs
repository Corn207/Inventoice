namespace Application.Exceptions;
public class OutOfStockException : Exception
{
	public OutOfStockException(string[] ids)
	{
		Ids = ids;
	}

	public string[] Ids { get; }
}
