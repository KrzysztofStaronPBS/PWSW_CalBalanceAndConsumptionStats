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
			PerformLoginAction();
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
		PerformLoginAction();
	}

	private async void PerformLoginAction()
	{
		var (success, message) = ViewModel.TryLogin();

		if (!success)
		{
			var dialog = new ContentDialog
			{
				Title = "B³¹d logowania",
				Content = message,
				CloseButtonText = "OK",
				XamlRoot = this.XamlRoot
			};

			await dialog.ShowAsync();
		}
	}
}