using System;
using Microsoft.UI.Xaml.Data;

namespace PWSW_CalBalanceAndConsumptionStats.Converters;

public class StringFormatConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, string language)
	{
		if (value == null) return string.Empty;

		string format = parameter as string;
		if (string.IsNullOrEmpty(format)) return value.ToString();

		return string.Format("{0:" + format + "}", value);
	}

	public object ConvertBack(object value, Type targetType, object parameter, string language)
		=> throw new NotImplementedException();
}