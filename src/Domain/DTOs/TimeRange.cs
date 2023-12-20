namespace Domain.DTOs;
public readonly record struct TimeRange
{
	public TimeRange(DateTime from, DateTime to)
	{
		if (from < to)
		{
			From = from;
			To = to;
		}
		else
		{
			From = to;
			To = from;
		}
	}

	public DateTime From { get; }
	public DateTime To { get; }
}
