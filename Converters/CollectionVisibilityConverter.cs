using System;
using System.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace PWSW_CalBalanceAndConsumptionStats.Converters;

public partial class CollectionVisibilityConverter : IValueConverter
{
	public bool Invert { get; set; }

	public object Convert(object value, Type targetType, object parameter, string language)
	{
		bool isEmpty = true;

		if (value is bool b) isEmpty = !b;
		if (value is int count) isEmpty = count == 0;
		else if (value is IEnumerable enumerable)
		{
			var enumerator = enumerable.GetEnumerator();
			isEmpty = !enumerator.MoveNext();
		}

		bool isVisible = Invert ? isEmpty : !isEmpty;
		return isVisible ? Visibility.Visible : Visibility.Collapsed;
	}

	public object ConvertBack(object value, Type targetType, object parameter, string language)
		=> throw new NotImplementedException();
}