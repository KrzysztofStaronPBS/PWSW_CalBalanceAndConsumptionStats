using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using PWSW_CalBalanceAndConsumptionStats.ViewModels;

namespace PWSW_CalBalanceAndConsumptionStats.Views.Pages;

public sealed partial class MainPage : Page
{
	public MainViewModel ViewModel { get; }

	public MainPage()
	{
		InitializeComponent();
		ViewModel = App.Current.Services.GetRequiredService<MainViewModel>();
	}

	protected override void OnNavigatedTo(NavigationEventArgs e)
	{
		base.OnNavigatedTo(e);
		ViewModel.UpdateDashboard();
	}
}