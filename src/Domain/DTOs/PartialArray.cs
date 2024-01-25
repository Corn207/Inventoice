namespace Domain.DTOs;
public readonly record struct PartialArray<T>(
	T[] Items,
	uint Total);

public record PartialEnumerable<T>(IEnumerable<T> Items, int Total)
{
	public PartialArray<TResult> ToPartialArray<TResult>(Func<T, TResult> select)
	{
		return new(Items.Select(select).ToArray(), Convert.ToUInt32(Total));
	}
}
