using System;
using Microsoft.UI.Xaml.Data;

namespace PWSW_CalBalanceAndConsumptionStats.Converters;

public partial class StringFormatConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, string language)
	{
		// jeśli wartość wejściowa jest nullem, zwracany jest pusty ciąg (bezpieczne dla UI)
		if (value == null) return string.Empty;

		string? format = parameter as string;

		if (string.IsNullOrEmpty(format))
			return value.ToString() ?? string.Empty;

		try
		{
			return string.Format(System.Globalization.CultureInfo.CurrentCulture, "{0:" + format + "}", value);
		}
		catch (FormatException)
		{
			return value.ToString() ?? string.Empty;
		}
	}

	public object ConvertBack(object value, Type targetType, object parameter, string language)
		=> throw new NotImplementedException();
}