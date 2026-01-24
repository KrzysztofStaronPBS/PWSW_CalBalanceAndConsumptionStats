using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using PWSW_CalBalanceAndConsumptionStats.Services;
using PWSW_CalBalanceAndConsumptionStats.Views.Pages;
using Windows.Graphics;
using WinRT.Interop;

namespace PWSW_CalBalanceAndConsumptionStats
{
	public sealed partial class MainWindow : Window
	{
		private AppWindow? _appWindow;

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
			IntPtr hWnd = WindowNative.GetWindowHandle(this);
			WindowId windowId = Win32Interop.GetWindowIdFromWindow(hWnd);

			_appWindow = AppWindow.GetFromWindowId(windowId);

			if (_appWindow is not null)
			{
				SizeInt32 windowSize = new(1200, 800);
				_appWindow.Resize(windowSize);
				CenterWindow(windowId);
			}
		}

		private void CenterWindow(WindowId windowId)
		{
			DisplayArea displayArea = DisplayArea.GetFromWindowId(windowId, DisplayAreaFallback.Nearest);

			if (displayArea != null && _appWindow is AppWindow window)
			{
				var centeredPosition = new PointInt32
				{
					X = (displayArea.WorkArea.Width - window.Size.Width) / 2,
					Y = (displayArea.WorkArea.Height - window.Size.Height) / 2
				};
				window.Move(centeredPosition);
			}
		}
	}
}