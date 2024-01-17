namespace MAUIApp.Services;

public class LocalizationService
{
	public static TimeZoneInfo LocalTimeZone { get; set; } = TimeZoneInfo.Local;

	public static DateTime ToLocalTime(DateTime dateTime)
	{
		var time = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
		return TimeZoneInfo.ConvertTimeFromUtc(time, LocalTimeZone);
	}
}
