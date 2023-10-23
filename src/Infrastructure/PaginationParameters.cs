namespace Infrastructure;
public readonly struct PaginationParameters
{
	public PaginationParameters(int pageSize, int pageNumber)
	{
		PageSize = pageSize;
		PageNumber = pageNumber;
	}

	public readonly int PageSize;
	public readonly int PageNumber;
}
