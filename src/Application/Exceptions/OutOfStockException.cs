namespace Application.Exceptions;
public class OutOfStockException(params string[] ids) : Exception
{
	public string[] Ids { get; } = ids;
}
