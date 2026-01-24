using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PWSW_CalBalanceAndConsumptionStats.Models;
using PWSW_CalBalanceAndConsumptionStats.Services;
using PWSW_CalBalanceAndConsumptionStats.Views.Pages;
using System;

namespace PWSW_CalBalanceAndConsumptionStats.ViewModels;

public partial class ProfileViewModel : ObservableObject
{
	private readonly DataManager _dataManager;
	private readonly NavigationService _navService;

	[ObservableProperty] private User? _currentUser;
	[ObservableProperty] private string _bmiStatus = string.Empty;
	[ObservableProperty] private string _displayGender = string.Empty;

	[ObservableProperty] private bool _isEditing;

	[ObservableProperty] private string _editWeight = string.Empty;
	[ObservableProperty] private string _editHeight = string.Empty;
	[ObservableProperty] private string _editAge = string.Empty;

	public ProfileViewModel(DataManager dataManager, NavigationService navService)
	{
		_dataManager = dataManager;
		_navService = navService;
		LoadUserData();
	}

	private void LoadUserData()
	{
		CurrentUser = _dataManager.CurrentUser;
		if (CurrentUser != null)
		{
			CurrentUser.CalculateBMI();
			DisplayGender = CurrentUser.Gender == Gender.M ? "Mężczyzna" : "Kobieta";
			UpdateBmiStatus();

			EditWeight = CurrentUser.Weight.ToString();
			EditHeight = CurrentUser.Height.ToString();
			EditAge = CurrentUser.Age.ToString();
		}
	}

	private void UpdateBmiStatus()
	{
		if (CurrentUser == null || CurrentUser.BMI == 0) { BmiStatus = "Brak danych"; return; }
		BmiStatus = CurrentUser.BMI switch
		{
			< 18.5 => "Niedowaga",
			< 25.0 => "Waga prawidłowa",
			< 30.0 => "Nadwaga",
			_ => "Otyłość"
		};
	}

	[RelayCommand] private void ToggleEdit() => IsEditing = !IsEditing;

	[RelayCommand]
	private void SaveChanges()
	{
		if (CurrentUser == null) return;

		// Walidacja formatu przed zapisem
		bool wOk = double.TryParse(EditWeight.Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double w);
		bool hOk = double.TryParse(EditHeight.Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double h);
		bool aOk = int.TryParse(EditAge, out int a);

		if (wOk && hOk && aOk)
		{
			CurrentUser.Weight = w;
			CurrentUser.Height = h;
			CurrentUser.Age = a;
			CurrentUser.CalculateBMI();

			_dataManager.SaveUserData(CurrentUser);
			IsEditing = false;
			LoadUserData();
		}
		else
		{ }
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