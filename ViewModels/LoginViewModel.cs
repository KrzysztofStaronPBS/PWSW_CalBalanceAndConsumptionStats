using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PWSW_CalBalanceAndConsumptionStats.Services;

namespace PWSW_CalBalanceAndConsumptionStats.ViewModels;

public partial class LoginViewModel : ObservableObject
{
	private readonly DataManager _dataManager;
	private readonly NavigationService _navService;

	[ObservableProperty] private string _loginInput = string.Empty;
	[ObservableProperty] private string _passwordInput = string.Empty;

	public LoginViewModel(DataManager dataManager, NavigationService navService)
	{
		_dataManager = dataManager;
		_navService = navService;
	}

	[RelayCommand]
	private void GoToRegister()
	{
		_navService.Navigate<Views.Pages.RegisterPage>();
	}

	// metoda zwraca (bool Success, string ErrorMessage)
	public (bool Success, string Message) TryLogin()
	{
		// walidacja wstępna
		if (string.IsNullOrWhiteSpace(LoginInput))
			return (false, "Podaj login.");

		if (string.IsNullOrWhiteSpace(PasswordInput))
			return (false, "Podaj hasło.");

		// sprawdzenie czy jakikolwiek użytkownik istnieje
		if (!_dataManager.HasUserData)
			return (false, "Brak profilu użytkownika na tym urządzeniu. Zarejestruj się.");

		// weryfikacja danych
		var user = _dataManager.LoadUserData();

		// sprawdzenie czy login się zgadza (case-insensitive dla wygody)
		if (!string.Equals(user.Name, LoginInput, System.StringComparison.OrdinalIgnoreCase))
			return (false, "Niepoprawny login.");

		// sprawdzamy hasło
		// TODO: w przyszłości warto dodać hashowanie!
		if (user.Password != PasswordInput)
			return (false, "Niepoprawne hasło.");

		// sukces - nawigacja do ekranu głównego
		_dataManager.CurrentUser = user;
		_navService.Navigate<Views.Pages.MainPage>();
		return (true, string.Empty);
	}
}