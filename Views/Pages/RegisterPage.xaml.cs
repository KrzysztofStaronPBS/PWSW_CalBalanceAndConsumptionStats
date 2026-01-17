using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Extensions.DependencyInjection;
using PWSW_CalBalanceAndConsumptionStats.ViewModels;
using PWSW_CalBalanceAndConsumptionStats.Services;

namespace PWSW_CalBalanceAndConsumptionStats.Views.Pages;

public sealed partial class RegisterPage : Page
{
	// udostêpnienie ViewModel dla x:Bind
	public RegisterViewModel ViewModel { get; }

	public RegisterPage()
	{
		InitializeComponent();

		ViewModel = App.Current.Services.GetRequiredService<RegisterViewModel>();
	}

	// obs³uga PasswordBox (zabezpieczenie WinUI utrudnia bindowanie, wiêc trzeba to zrobiæ rêcznie)
	private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
	{
		ViewModel.Password = PasswordBox.Password;
	}

	// klikniêcie "Utwórz konto"
	private async void Register_Click(object sender, RoutedEventArgs e)
	{
		var result = ViewModel.RegisterUser();

		if (!result.Success)
		{
			var dialog = new ContentDialog
			{
				Title = "B³¹d rejestracji",
				Content = result.Message,
				CloseButtonText = "Popraw dane",
				XamlRoot = this.XamlRoot
			};
			await dialog.ShowAsync();
		}
		else
		{
		}
	}

	private void Cancel_Click(object sender, RoutedEventArgs e)
	{
		// powrót do logowania
		var nav = App.Current.Services.GetRequiredService<NavigationService>();
		nav.Navigate<LoginPage>();
	}
}