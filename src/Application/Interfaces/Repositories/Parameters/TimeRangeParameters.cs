namespace Application.Interfaces.Repositories.Parameters;
public readonly struct TimeRangeParameters
{
	public TimeRangeParameters(DateTime from, DateTime to)
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

	public readonly DateTime From;
	public readonly DateTime To;
}
