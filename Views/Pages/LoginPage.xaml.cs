using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using PWSW_CalBalanceAndConsumptionStats.ViewModels;
using Windows.System;

namespace PWSW_CalBalanceAndConsumptionStats.Views.Pages;

public sealed partial class LoginPage : Page
{
	public LoginViewModel ViewModel { get; }

	public LoginPage()
	{
		InitializeComponent();
		ViewModel = App.Current.Services.GetRequiredService<LoginViewModel>();
	}

	private void OnKeyDown(object sender, KeyRoutedEventArgs e)
	{
		if (e.Key == VirtualKey.Enter)
		{
			Login_Click(this, null);
			e.Handled = true;
		}
	}

	private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
	{
		if (ViewModel != null)
		{
			ViewModel.PasswordInput = PasswordBox.Password;
		}
	}

	private async void Login_Click(object sender, RoutedEventArgs e)
	{
		// wywo³anie logiki logowania
		var result = ViewModel.TryLogin();

		if (!result.Success)
		{
			// jeœli b³¹d, show dialog
			var dialog = new ContentDialog
			{
				Title = "B³¹d logowania",
				Content = result.Message,
				CloseButtonText = "OK",
				XamlRoot = this.XamlRoot
			};

			await dialog.ShowAsync();
		}
	}
}