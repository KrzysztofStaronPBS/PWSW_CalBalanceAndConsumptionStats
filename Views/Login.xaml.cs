using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Win32;

namespace Win.Views
{
	public sealed partial class Login : Window
	{
		public User? LoggedUser { get; private set; }
		public DataManager? Data { get; private set; }

		public Login()
		{
			this.InitializeComponent();
		}

		private async void Login_Click(object sender, RoutedEventArgs e)
		{
			string login = LoginBox.Text.Trim();
			string password = PasswordBox.Password.Trim();

			// WALIDACJA LOGINU
			if (string.IsNullOrWhiteSpace(login))
			{
				await ShowDialog("B³¹d", "Login nie mo¿e byæ pusty.");
				return;
			}

			if (login.Length < 3)
			{
				await ShowDialog("B³¹d", "Login musi mieæ co najmniej 3 znaki.");
				return;
			}

			if (!Regex.IsMatch(login, @"^[a-zA-Z0-9]+$"))
			{
				await ShowDialog("B³¹d", "Login mo¿e zawieraæ tylko litery i cyfry.");
				return;
			}

			// WALIDACJA HAS£A
			if (string.IsNullOrWhiteSpace(password))
			{
				await ShowDialog("B³¹d", "Has³o nie mo¿e byæ puste.");
				return;
			}

			if (password.Length < 8)
			{
				await ShowDialog("B³¹d", "Has³o musi mieæ co najmniej 8 znaków.");
				return;
			}

			string userDir = Path.Combine("Users", login);

			if (!Directory.Exists(userDir))
			{
				await ShowDialog("B³¹d", "U¿ytkownik nie istnieje.");
				return;
			}

			Data = new DataManager(userDir);
			LoggedUser = Data.LoadUserData();

			if (LoggedUser.Password != password)
			{
				await ShowDialog("B³¹d", "Niepoprawne has³o.");
				return;
			}

			this.Close();
		}

		private async void Register_Click(object sender, RoutedEventArgs e)
		{
			var registerWindow = new Register();
			registerWindow.Activate();

			// czekamy a¿ u¿ytkownik zamknie okno rejestracji
			registerWindow.Closed += async (_, _) =>
			{
				if (registerWindow.CreatedUser != null)
				{
					// automatyczne logowanie po rejestracji
					LoggedUser = registerWindow.CreatedUser;
					Data = registerWindow.Data;

					await ShowDialog("Sukces", "Konto utworzono pomyœlnie.");
					this.Close();
				}
			};
		}

		private async Task ShowDialog(string title, string message)
		{
			ContentDialog dialog = new ContentDialog()
			{
				Title = title,
				Content = message,
				CloseButtonText = "OK",
				XamlRoot = this.Content.XamlRoot
			};

			await dialog.ShowAsync();
		}
	}
}
