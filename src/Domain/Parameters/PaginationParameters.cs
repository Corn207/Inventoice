namespace Domain.Parameters;
public readonly struct PaginationParameters
{
	public PaginationParameters(ushort pageNumber, ushort pageSize)
	{
		if (pageNumber < 1)
			throw new ArgumentOutOfRangeException(nameof(pageNumber));
		if (pageSize < 1)
			throw new ArgumentOutOfRangeException(nameof(pageSize));

		PageNumber = pageNumber;
		PageSize = pageSize;
	}

	public readonly ushort PageNumber;
	public readonly ushort PageSize;
}
