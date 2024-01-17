using CommunityToolkit.Maui.Converters;
using System.Globalization;

namespace MAUIApp.Converters;

public class TotalProductToRemainingConverter : BaseConverterOneWay<uint?, uint?>
{
	public override uint? DefaultConvertReturnValue { get; set; } = null;

	public override uint? ConvertFrom(uint? value, CultureInfo? culture)
	{
		if (value == 0) return null;
		return value - 1;
	}
}
