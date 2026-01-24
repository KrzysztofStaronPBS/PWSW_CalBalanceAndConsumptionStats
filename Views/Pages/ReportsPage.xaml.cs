using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using PWSW_CalBalanceAndConsumptionStats.ViewModels;

namespace PWSW_CalBalanceAndConsumptionStats.Views.Pages;

public sealed partial class ReportsPage : Page
{
	public ReportsViewModel ViewModel { get; }

	public ReportsPage()
	{
		this.InitializeComponent();
		this.ViewModel = App.Current.Services.GetRequiredService<ReportsViewModel>();
	}
}