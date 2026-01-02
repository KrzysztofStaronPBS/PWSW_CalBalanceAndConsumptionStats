using Microsoft.UI.Xaml;
using Win.Views;

namespace Win
{
	public sealed partial class MainWindow : Window
	{
		private bool _loginShown = false;

		public MainWindow()
		{
			InitializeComponent();
			Activated += MainWindow_Activated;
		}

		private void MainWindow_Activated(object sender, WindowActivatedEventArgs e)
		{
			if (_loginShown)
				return;

			_loginShown = true;

			var loginWindow = new Login();
			loginWindow.Activate();

			loginWindow.Closed += (_, _) =>
			{
				if (loginWindow.LoggedUser == null || loginWindow.Data == null)
				{
					Close();
					return;
				}

				var mainPage = new MainPage(loginWindow.LoggedUser, loginWindow.Data);
				mainPage.Activate();

				Close();
			};
		}
	}
}
