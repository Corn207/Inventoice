using CommunityToolkit.Maui.Converters;
using MAUIApp.Services;
using System.Globalization;

namespace MAUIApp.Converters;

public sealed class DateTimeToWeekDayStringConverter : BaseConverterOneWay<DateTime?, string?>
{
	public override string? DefaultConvertReturnValue { get; set; } = null;

	public override string? ConvertFrom(DateTime? value, CultureInfo? culture)
	{
		if (!value.HasValue) return null;
		var local = LocalizationService.ToLocalTime(value.Value);
		var result = local.ToString("dddd, dd/MM/yyyy", new CultureInfo("vi-VN"));

		return result;
	}
}
