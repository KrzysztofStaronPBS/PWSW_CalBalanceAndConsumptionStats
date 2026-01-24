using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using PWSW_CalBalanceAndConsumptionStats.ViewModels;

namespace PWSW_CalBalanceAndConsumptionStats.Views.Pages
{
	public sealed partial class AddEntryPage : Page
	{
		public AddEntryViewModel ViewModel { get; }
		public AddEntryPage()
		{
			this.InitializeComponent();
			this.ViewModel = App.Current.Services.GetRequiredService<AddEntryViewModel>();
		}
	}
}
