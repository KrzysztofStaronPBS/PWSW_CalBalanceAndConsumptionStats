using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.Extensions.DependencyInjection;
using PWSW_CalBalanceAndConsumptionStats.Services;
using PWSW_CalBalanceAndConsumptionStats.Views.Pages;
using Windows.Graphics;
using WinRT.Interop;

namespace PWSW_CalBalanceAndConsumptionStats
{
	public sealed partial class MainWindow : Window
	{
		private AppWindow _appWindow;

		public MainWindow()
		{
			this.InitializeComponent();
			InitializeWindow();

			var navService = App.Current.Services.GetRequiredService<NavigationService>();
			navService.Initialize(AppFrame);
			navService.Navigate<LoginPage>();
		}

		private void InitializeWindow()
		{
			System.IntPtr hWnd = WindowNative.GetWindowHandle(this);
			WindowId windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
			_appWindow = AppWindow.GetFromWindowId(windowId);

			if (_appWindow != null)
			{
				SizeInt32 windowSize = new SizeInt32(1200, 800);
				_appWindow.Resize(windowSize);

				// œrodkowanie okna wzglêdem ekranu roboczego
				CenterWindow(windowId);
			}
		}

		private void CenterWindow(WindowId windowId)
		{
			DisplayArea displayArea = DisplayArea.GetFromWindowId(windowId, DisplayAreaFallback.Nearest);
			if (displayArea != null)
			{
				var centeredPosition = new PointInt32
				{
					X = (displayArea.WorkArea.Width - _appWindow.Size.Width) / 2,
					Y = (displayArea.WorkArea.Height - _appWindow.Size.Height) / 2
				};
				_appWindow.Move(centeredPosition);
			}
		}
	}
}