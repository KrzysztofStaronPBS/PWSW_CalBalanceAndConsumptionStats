using Microsoft.UI.Xaml.Controls;
using System;

namespace PWSW_CalBalanceAndConsumptionStats.Services;

public class NavigationService
{
	private Frame? _frame;

	public void Initialize(Frame frame)
	{
		_frame = frame;
	}

	// metoda do nawigacji, wołana z ViewModeli
	public bool Navigate(Type pageType, object? parameter = null)
	{
		if (_frame == null) return false;

		// nie nawiguj, jeśli user jest już na tej stronie
		if (_frame.Content?.GetType() == pageType) return false;

		return _frame.Navigate(pageType, parameter);
	}

	public bool Navigate<T>(object? parameter = null)
	{
		return Navigate(typeof(T), parameter);
	}

	public void GoBack()
	{
		if (_frame != null && _frame.CanGoBack)
		{
			_frame.GoBack();
		}
	}
}