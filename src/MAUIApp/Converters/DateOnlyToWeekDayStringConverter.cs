using CommunityToolkit.Maui.Converters;
using System.Globalization;

namespace MAUIApp.Converters;

public sealed class DateOnlyToWeekDayStringConverter : BaseConverterOneWay<DateOnly?, string?>
{
	public override string? DefaultConvertReturnValue { get; set; } = null;

	public override string? ConvertFrom(DateOnly? value, CultureInfo? culture)
	{
		if (!value.HasValue) return null;
		var result = value.Value.ToString("dddd, dd/MM/yyyy", new CultureInfo("vi-VN"));

		return result;
	}
}
