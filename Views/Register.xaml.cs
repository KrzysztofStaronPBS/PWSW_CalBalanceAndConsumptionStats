using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Win.Views
{
	public sealed partial class Register : Window
	{
		public User? CreatedUser { get; private set; }
		public DataManager? Data { get; private set; }

		public Register()
		{
			this.InitializeComponent();
		}

		private async void Register_Click(object sender, RoutedEventArgs e)
		{
			string login = LoginBox.Text.Trim();
			string password = PasswordBox.Password.Trim();

			// login
			if (string.IsNullOrWhiteSpace(login) || login.Length < 3)
			{
				await ShowDialog("Błąd", "Login musi mieć co najmniej 3 znaki.");
				return;
			}

			if (!Regex.IsMatch(login, @"^[a-zA-Z0-9]+$"))
			{
				await ShowDialog("Błąd", "Login może zawierać tylko litery i cyfry.");
				return;
			}

			// hasło
			if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
			{
				await ShowDialog("Błąd", "Hasło musi mieć co najmniej 8 znaków.");
				return;
			}

			// wiek
			if (!int.TryParse(AgeBox.Text, out int age) || age < 1 || age > 120)
			{
				await ShowDialog("Błąd", "Wiek musi być liczbą z zakresu 1–120.");
				return;
			}

			// płeć
			if (GenderBox.SelectedItem is not ComboBoxItem genderItem)
			{
				await ShowDialog("Błąd", "Wybierz płeć.");
				return;
			}

			string gender = genderItem.Content.ToString()!;

			// wzrost
			if (!double.TryParse(HeightBox.Text, out double height) || height <= 0 || height > 3)
			{
				await ShowDialog("Błąd", "Wzrost musi być liczbą w metrach (np. 1.75).");
				return;
			}

			// masa ciała
			if (!double.TryParse(WeightBox.Text, out double weight) || weight <= 0 || weight > 250)
			{
				await ShowDialog("Błąd", "Masa musi być liczbą z zakresu 1–250 kg.");
				return;
			}

			// cel kaloryczny
			int goalCalories = 0;

			if (GoalTypeBox.SelectedItem is not ComboBoxItem goalItem)
			{
				await ShowDialog("Błąd", "Wybierz cel.");
				return;
			}

			string goalChoice = goalItem.Content?.ToString() ?? "";

			// utrzymanie wagi -> cel = 0
			if (goalChoice == "Chcę utrzymać wagę")
			{
				goalCalories = 0;
			}
			else
			{
				// dla schudnięcia i przytycia potrzebna liczba
				if (!int.TryParse(GoalValueBox.Text, out int value) || value <= 0)
				{
					await ShowDialog("Błąd", "Podaj dodatnią liczbę kalorii.");
					return;
				}

				if (goalChoice == "Chcę schudnąć")
					goalCalories = -value;   // ujemny cel

				if (goalChoice == "Chcę przytyć")
					goalCalories = value;    // dodatni cel
			}


			// czy user istnieje
			string userDir = Path.Combine("Users", login);

			if (Directory.Exists(userDir))
			{
				await ShowDialog("Błąd", "Taki użytkownik już istnieje.");
				return;
			}

			Directory.CreateDirectory(userDir);

			Data = new DataManager(userDir);

			// tworzenie użytkownika
			CreatedUser = new User
			{
				Id = 1,
				Name = login,
				Password = password,
				Age = age,
				Gender = gender,
				Height = height,
				Weight = weight,
				GoalCalories = goalCalories,
			};

			CreatedUser.CalculateBMI();

			Data.SaveUserData(CreatedUser);

			await ShowDialog("Sukces", "Konto zostało utworzone.");
			this.Close();
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

		private void GoalTypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (GoalTypeBox.SelectedItem is ComboBoxItem item)
			{
				string choice = item.Content?.ToString() ?? "";

				// jeśli użytkownik chce schudnąć lub przytyć → pokaż pole "Ile?"
				if (choice == "Chcę schudnąć" || choice == "Chcę przytyć")
				{
					GoalValuePanel.Visibility = Visibility.Visible;
				}
				else
				{
					GoalValuePanel.Visibility = Visibility.Collapsed;
					GoalValueBox.Text = "";
				}
			}
		}
	}
}
