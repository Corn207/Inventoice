using System.Globalization;

namespace MAUIApp.Converters;

public class TotalPriceToCurrencyStringConverter : IMultiValueConverter
{
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		var casted0 = System.Convert.ToUInt32(values[0]);
		var casted1 = System.Convert.ToUInt32(values[1]);
		var result = (casted0 * casted1).ToString("N0", CultureInfo.InvariantCulture);

		return result;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException(nameof(TotalPriceToCurrencyStringConverter));
	}
}
