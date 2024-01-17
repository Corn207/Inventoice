using CommunityToolkit.Maui.Converters;
using System.Globalization;

namespace MAUIApp.Converters;

public class UIntToCurrencyStringConverter : BaseConverterOneWay<object?, string?>
{
	public override string? DefaultConvertReturnValue { get; set; } = null;

	public override string? ConvertFrom(object? value, CultureInfo? culture)
	{
		if (value is null) return null;
		uint casted;
		try
		{
			casted = Convert.ToUInt32(value);
		}
		catch (Exception)
		{
			throw;
		}
		var result = casted.ToString("N0", CultureInfo.InvariantCulture);

		return result;
	}
}
