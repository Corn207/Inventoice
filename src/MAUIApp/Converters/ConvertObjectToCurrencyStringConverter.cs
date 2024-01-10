using CommunityToolkit.Maui.Converters;
using System.Globalization;

namespace MAUIApp.Converters;

public class ConvertObjectToCurrencyStringConverter : BaseConverterOneWay<object?, string?>
{
	public override string? DefaultConvertReturnValue { get; set; } = null;

	public override string? ConvertFrom(object? value, CultureInfo? culture)
	{
		if (value is null) return null;
		var casted = Convert.ToUInt32(value);
		var result = casted.ToString("N0", CultureInfo.InvariantCulture);

		return result;
	}
}
