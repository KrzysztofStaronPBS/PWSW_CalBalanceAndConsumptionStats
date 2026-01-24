using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PWSW_CalBalanceAndConsumptionStats.Models;
using PWSW_CalBalanceAndConsumptionStats.Services;
using PWSW_CalBalanceAndConsumptionStats.Views.Pages;

namespace PWSW_CalBalanceAndConsumptionStats.ViewModels;

public partial class ProfileViewModel : ObservableObject
{
	private readonly DataManager _dataManager;
	private readonly NavigationService _navService;

	[ObservableProperty] private User? _currentUser;
	[ObservableProperty] private string _bmiStatus = string.Empty;
	[ObservableProperty] private string _displayGender = string.Empty;

	public ProfileViewModel(DataManager dataManager, NavigationService navService)
	{
		_dataManager = dataManager;
		_navService = navService;

		CurrentUser = _dataManager.CurrentUser;

		if (CurrentUser != null)
		{
			CurrentUser.CalculateBMI();

			DisplayGender = CurrentUser.Gender switch
			{
				Gender.M => "Mężczyzna",
				Gender.K => "Kobieta",
				_ => "Nieokreślona"
			};

			UpdateBmiStatus();
		}
	}

	private void UpdateBmiStatus()
	{
		if (CurrentUser == null || CurrentUser.BMI == 0)
		{
			BmiStatus = "Brak danych";
			return;
		}

		BmiStatus = CurrentUser.BMI switch
		{
			< 18.5 => "Niedowaga",
			< 25.0 => "Waga prawidłowa",
			< 30.0 => "Nadwaga",
			_ => "Otyłość"
		};
	}

	[RelayCommand]
	private void GoBack() => _navService.Navigate<MainPage>();

	[RelayCommand]
	private void Logout()
	{
		_dataManager.CurrentUser = null;
		_navService.Navigate<LoginPage>();
	}

	[RelayCommand]
	private void DeleteAccount()
	{
		_dataManager.DeleteAccount();
		_navService.Navigate<LoginPage>();
	}
}