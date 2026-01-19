using System;
using Microsoft.UI;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using PWSW_CalBalanceAndConsumptionStats.Models;

namespace PWSW_CalBalanceAndConsumptionStats.Converters;

public class CalorieColorConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, string language)
	{
		if (value is Models.EntryType type)
		{
			return type == Models.EntryType.Meal
				? new SolidColorBrush(Microsoft.UI.Colors.OrangeRed)
				: new SolidColorBrush(Microsoft.UI.Colors.MediumSeaGreen);
		}
		return new SolidColorBrush(Microsoft.UI.Colors.Gray);
	}
	public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}