using Microsoft.UI.Xaml;

namespace Win.Views
{
	public sealed partial class MainPage : Window
	{
		private readonly User _user;
		private readonly DataManager _data;

		public MainPage(User user, DataManager data)
		{
			InitializeComponent();
			_user = user;
			_data = data;
		}
	}
}
