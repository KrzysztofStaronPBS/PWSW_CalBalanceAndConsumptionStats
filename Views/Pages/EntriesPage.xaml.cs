using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using PWSW_CalBalanceAndConsumptionStats.ViewModels;

namespace PWSW_CalBalanceAndConsumptionStats.Views.Pages;

public sealed partial class EntriesPage : Page
{
	public EntriesViewModel ViewModel { get; }

	public EntriesPage()
	{
		this.InitializeComponent();
		this.ViewModel = App.Current.Services.GetRequiredService<EntriesViewModel>();
	}

	protected override void OnNavigatedTo(NavigationEventArgs e)
	{
		base.OnNavigatedTo(e);
		ViewModel.LoadDataCommand.Execute(null);
	}
}