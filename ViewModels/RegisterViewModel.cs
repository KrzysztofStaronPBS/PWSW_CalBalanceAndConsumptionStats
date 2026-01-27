using System;
using System.Globalization;
using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.ComponentModel;
using PWSW_CalBalanceAndConsumptionStats.Models;
using PWSW_CalBalanceAndConsumptionStats.Services;
using PWSW_CalBalanceAndConsumptionStats.Views.Pages;

namespace PWSW_CalBalanceAndConsumptionStats.ViewModels;

public partial class RegisterViewModel(DataManager dataManager, NavigationService navService) : ObservableObject
{
	private readonly DataManager _dataManager = dataManager;
	private readonly NavigationService _navService = navService;

	[GeneratedRegex(@"^[a-zA-Z0-9]+$")]
	private static partial Regex LoginRegex();

	[ObservableProperty] private string _login = string.Empty;
	[ObservableProperty] private string _password = string.Empty;
	[ObservableProperty] private string _ageText = string.Empty;
	[ObservableProperty] private string _heightText = string.Empty;
	[ObservableProperty] private string _weightText = string.Empty;
	[ObservableProperty] private int _genderIndex = 0;
	[ObservableProperty] private int _activityIndex = 0;

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(IsGoalValueVisible))]
	private int _goalTypeIndex = 0;

	[ObservableProperty] private string _goalValueText = string.Empty;

	public bool IsGoalValueVisible => GoalTypeIndex == 1 || GoalTypeIndex == 3;

	public (bool Success, string Message) RegisterUser()
	{
		// walidacja danych wejściowych
		var validation = ValidateInput();
		if (!validation.Success) return validation;

		try
		{
			// sprawdzenie czy użytkownik o takim LOGINIE już istnieje
			if (_dataManager.DoesUserExist(Login))
				return (false, "Użytkownik o tym loginie już istnieje. Wybierz inną nazwę.");

			// mapowanie pól do modelu User
			var newUser = MapFieldsToUser();

			// inicjalizacja struktury katalogów i zapis (DataManager)
			// to utworzy folder /UserData/Login/ oraz pliki JSON
			_dataManager.InitializeUserStructure(newUser.Name, newUser);

			// nawigacja do MainPage
			_navService.Navigate<MainPage>();

			return (true, "Konto utworzone pomyślnie.");
		}
		catch (Exception ex)
		{
			return (false, $"Błąd krytyczny podczas rejestracji: {ex.Message}");
		}
	}

	private (bool Success, string Message) ValidateInput()
	{
		if (string.IsNullOrWhiteSpace(Login) || Login.Length < 3)
			return (false, "Login musi mieć co najmniej 3 znaki.");

		if (!LoginRegex().IsMatch(Login))
			return (false, "Login może zawierać tylko litery i cyfry.");

		if (string.IsNullOrWhiteSpace(Password) || Password.Length < 8)
			return (false, "Hasło musi mieć co najmniej 8 znaków.");

		if (!int.TryParse(AgeText, out int age) || age < 1 || age > 120)
			return (false, "Wiek musi być liczbą (1-120).");

		if (GenderIndex == 0)
			return (false, "Wybierz płeć.");

		// obsługa regionalna kropki/przecinka
		string h = HeightText.Replace(',', '.');
		string w = WeightText.Replace(',', '.');

		if (!double.TryParse(h, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double height) || height <= 0 || height > 3)
			return (false, "Wzrost musi być w metrach (np. 1.75).");

		if (!double.TryParse(w, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double weight) || weight <= 0 || weight > 250)
			return (false, "Waga musi być z zakresu 1-250 kg.");

		if (GoalTypeIndex == 0)
			return (false, "Wybierz cel.");

		return (true, string.Empty);
	}

	private User MapFieldsToUser()
	{
		// dane są poprawne, bo metoda wywoływana po ValidateInput
		_ = int.TryParse(AgeText, out int age);
		_ = double.TryParse(HeightText.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double height);
		_ = double.TryParse(WeightText.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double weight);

		double pal = ActivityIndex switch
		{
			1 => 1.2,   // siedzący
			2 => 1.375, // niska aktywność
			3 => 1.55,  // średnia
			4 => 1.725, // wysoka
			_ => 1.2
		};

		int goalOffset = 0;
		if (int.TryParse(GoalValueText, out int val))
		{
			goalOffset = (GoalTypeIndex == 1) ? -val : (GoalTypeIndex == 3 ? val : 0);
		}

		var user = new User
		{
			Id = 1,
			Name = Login,
			Password = Password,
			Age = age,
			Gender = GenderIndex == 1 ? Gender.K : Gender.M,
			Height = height,
			Weight = weight,
			ActivityLevel = pal,
			DailyGoalOffset = goalOffset
		};
		user.CalculateBMI();
		return user;
	}
}