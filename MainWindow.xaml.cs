using Microsoft.UI.Xaml;
using Microsoft.Extensions.DependencyInjection;
using PWSW_CalBalanceAndConsumptionStats.Services;
using PWSW_CalBalanceAndConsumptionStats.Views.Pages;

namespace PWSW_CalBalanceAndConsumptionStats
{
	public sealed partial class MainWindow : Window
	{
		public MainWindow()
		{
			this.InitializeComponent();

			// pobieranie NavigationService z kontenera DI
			var navService = App.Current.Services.GetRequiredService<NavigationService>();

			// inicjalizacja ramk¹
			navService.Initialize(AppFrame);

			// wymuszenie startu od strony logowania
			navService.Navigate<LoginPage>();
		}
	}
}