using Microsoft.UI.Xaml.Controls;
using Microsoft.Extensions.DependencyInjection;
using PWSW_CalBalanceAndConsumptionStats.ViewModels;

namespace PWSW_CalBalanceAndConsumptionStats.Views.Pages;

public sealed partial class CatalogEditorPage : Page
{
	public CatalogEditorViewModel ViewModel { get; }

	public CatalogEditorPage()
	{
		this.InitializeComponent();
		this.ViewModel = App.Current.Services.GetRequiredService<CatalogEditorViewModel>();
	}

	public string GetValueLabel(int index) => index == 0 ? "kcal/100g" : "MET";
}