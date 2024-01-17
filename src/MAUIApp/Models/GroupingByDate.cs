using System.Collections;

namespace MAUIApp.Models;

public class GroupingByDate<T>(DateTime key, IEnumerable<T> values) : IGrouping<DateTime, T>
{
	private readonly IEnumerable<T> _values = values;
	public DateTime Key { get; } = key;

	public IEnumerator<T> GetEnumerator()
	{
		return _values.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return _values.GetEnumerator();
	}
}
