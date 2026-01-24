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
		if (string.IsNullOrWhiteSpace(LoginInput))
			return (false, "Podaj login.");

		if (string.IsNullOrWhiteSpace(PasswordInput))
			return (false, "Podaj hasło.");

		// sprawdzenie czy folder i plik istnieją dla tego konkretnego loginu
		if (!_dataManager.DoesUserExist(LoginInput))
		{
			return (false, "Użytkownik o podanym loginie nie istnieje.");
		}

		// wczytanie danych tego konkretnego użytkownika
		var user = _dataManager.LoadSpecificUser(LoginInput);

		if (user == null)
			return (false, "Błąd podczas wczytywania danych profilu.");

		// weryfikacja hasła
		if (user.Password != PasswordInput)
			return (false, "Niepoprawne hasło.");

		// ustawienie sesji
		_dataManager.CurrentUser = user;

		_navService.Navigate<Views.Pages.MainPage>();
		return (true, string.Empty);
	}
}