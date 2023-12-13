namespace Domain.DTOs;
public readonly record struct Pagination
{
    public Pagination(ushort pageNumber, ushort pageSize)
    {
        if (pageNumber < 1)
            throw new ArgumentOutOfRangeException(nameof(pageNumber));
        if (pageSize < 1)
            throw new ArgumentOutOfRangeException(nameof(pageSize));

        PageNumber = pageNumber;
        PageSize = pageSize;
    }

    public ushort PageNumber { get; }
    public ushort PageSize { get; }
}
