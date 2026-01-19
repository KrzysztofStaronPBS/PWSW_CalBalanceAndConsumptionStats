using Microsoft.UI.Xaml.Controls;
using Microsoft.Extensions.DependencyInjection;
using PWSW_CalBalanceAndConsumptionStats.ViewModels;

namespace PWSW_CalBalanceAndConsumptionStats.Views.Pages;

public sealed partial class ProfilePage : Page
{
	public ProfileViewModel ViewModel { get; }

	public ProfilePage()
	{
		this.InitializeComponent();
		ViewModel = App.Current.Services.GetRequiredService<ProfileViewModel>();
	}
}