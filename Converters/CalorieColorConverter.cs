using System;
using Microsoft.UI;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;

namespace PWSW_CalBalanceAndConsumptionStats.Converters;

public partial class CalorieColorConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, string language)
	{
		if (value is Models.EntryType type)
		{
			return type == Models.EntryType.Meal
				? new SolidColorBrush(Colors.OrangeRed)
				: new SolidColorBrush(Colors.MediumSeaGreen);
		}
		return new SolidColorBrush(Colors.Gray);
	}
	public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}