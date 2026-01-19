using System;
using Microsoft.UI.Xaml.Data;

namespace PWSW_CalBalanceAndConsumptionStats.Converters;

public class TypeToIconConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, string language)
	{
		if (value is Models.EntryType type)
		{
			// ikony jedzenia i sportu z Segoe MDL2
			return type == Models.EntryType.Meal ? "\uE95E" : "\uEADA";
		}
		return "\uE9CE";
	}
	public object ConvertBack(
		object value, Type targetType, object parameter, string language
		) => throw new NotImplementedException();
}