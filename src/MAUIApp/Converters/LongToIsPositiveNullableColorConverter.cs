using CommunityToolkit.Maui.Converters;
using System.Globalization;

namespace MAUIApp.Converters;

public sealed class LongToIsPositiveNullableColorConverter : BaseConverterOneWay<object?, Color?>
{
	public override Color? DefaultConvertReturnValue { get; set; } = null;

	public override Color? ConvertFrom(object? value, CultureInfo? culture)
	{
		if (value is null)
		{
			return null;
		}

		long casted;
		try
		{
			casted = Convert.ToInt64(value);
		}
		catch (Exception)
		{
			throw;
		}

		if (casted == 0)
		{
			return GetColor("TextPrimary");
		}
		else if (casted > 0)
		{
			return GetColor("Green");
		}
		else
		{
			return GetColor("Red");
		}
	}

	private static Color GetColor(string resourceName)
	{
		if (Application.Current is null)
		{
			throw new InvalidOperationException("Application.Current is null");
		}

		if (!Application.Current.Resources.TryGetValue(resourceName, out var colorvalue))
		{
			throw new ArgumentException($"Resource {resourceName} is not a color");
		}

		return (Color)colorvalue;
	}
}
