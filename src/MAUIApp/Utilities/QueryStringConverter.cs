using System.Web;

namespace MAUIApp.Utilities;
public sealed class QueryStringConverter
{
	public static string Convert(IDictionary<string, object?> queries, bool hasLeadingChar = false)
	{
		if (queries.Count > 0)
		{
			var queryStrings = queries
				.Where(x => x.Value is not null)
				.Select(x => Convert(x.Key, x.Value));
			var query = string.Join('&', queryStrings);
			var result = (hasLeadingChar ? "?" : string.Empty) + query;
			return result;
		}
		else
		{
			return string.Empty;
		}
	}

	public static string Convert<T>(string key, T value)
	{
		if (value is null)
		{
			return string.Empty;
		}

		if (value is DateTime dateTime)
		{
			return Convert(key, dateTime);
		}
		else
		{
			var result = HttpUtility.UrlEncode(value.ToString());
			return $"{key}={result}";
		}
	}

	public static string Convert(string key, DateTime value)
	{
		var result = HttpUtility.UrlEncode(value.ToString("s"));
		return $"{key}={result}";
	}
}
