using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace PWSW_CalBalanceAndConsumptionStats.Converters;

public class BooleanToVisibilityConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, string language)
	{
		if (value is bool b && b) return Visibility.Visible;
		return Visibility.Collapsed;
	}

	public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}