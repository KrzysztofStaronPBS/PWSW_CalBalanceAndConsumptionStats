using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using PWSW_CalBalanceAndConsumptionStats.Catalogs;
using PWSW_CalBalanceAndConsumptionStats.Services;
using PWSW_CalBalanceAndConsumptionStats.ViewModels;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Path = System.IO.Path;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PWSW_CalBalanceAndConsumptionStats
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : Application
    {
        private Window? _window;
		public IServiceProvider Services { get; }
		public new static App Current => (App)Application.Current;

		/// <summary>
		/// Initializes the singleton application object.  This is the first line of authored code
		/// executed, and as such is the logical equivalent of main() or WinMain().
		/// </summary>
		public App()
        {
            InitializeComponent();
			Services = ConfigureServices();
		}

		private static IServiceProvider ConfigureServices()
		{
			var services = new ServiceCollection();

			string localPath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "UserData");

			services.AddSingleton(new DataManager(localPath));

			services.AddSingleton<NavigationService>();
			services.AddTransient<ReportGenerator>();
			services.AddTransient<MainViewModel>();
			services.AddTransient<LoginViewModel>();
			services.AddTransient<RegisterViewModel>();
			services.AddSingleton<MealCatalog>();
			services.AddSingleton<ActivityCatalog>();

			return services.BuildServiceProvider();
		}

		/// <summary>
		/// Invoked when the application is launched.
		/// </summary>
		/// <param name="args">Details about the launch request and process.</param>
		protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            _window = new MainWindow();
            _window.Activate();
        }
    }
}
