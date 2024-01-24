using System.Collections;

namespace Domain.DTOs;
public readonly record struct PartialArray<T>(
	T[] Items,
	uint Total);

public class PartialEnumerable<T>(IEnumerable<T> enumerable, uint total) : IEnumerable<T>
{
	public IEnumerable<T> Enumerable { get; } = enumerable;
	public uint Total { get; } = total;

	public IEnumerator<T> GetEnumerator()
	{
		return Enumerable.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public PartialArray<TReturn> ToPartialArray<TReturn>(Func<T, TReturn> select)
	{
		return new PartialArray<TReturn>(Enumerable.Select(select).ToArray(), Total);
	}
}
