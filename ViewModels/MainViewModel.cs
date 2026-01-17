using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PWSW_CalBalanceAndConsumptionStats.Services;

namespace PWSW_CalBalanceAndConsumptionStats.ViewModels;

public partial class MainViewModel : ObservableObject
{
	private readonly NavigationService _navService;
	private readonly DataManager _dataManager;

	// powitanie
	[ObservableProperty]
	private string _welcomeMessage = "Witaj!";

	public MainViewModel(NavigationService navService, DataManager dataManager)
	{
		_navService = navService;
		_dataManager = dataManager;

		if (_dataManager.CurrentUser != null)
		{
			WelcomeMessage = $"Witaj, {_dataManager.CurrentUser.Name}!";
		}
	}

	[RelayCommand]
	private void GoToEntries()
	{
		// _navService.Navigate<Views.Pages.EntriesPage>();
	}

	[RelayCommand]
	private void GoToMeals()
	{
		// _navService.Navigate<Views.Pages.MealsPage>();
	}

	[RelayCommand]
	private void GoToReports()
	{
		// _navService.Navigate<Views.Pages.ReportsPage>();
	}

	[RelayCommand]
	private void GoToDailySummary()
	{
		// _navService.Navigate<Views.Pages.DailySummaryPage>();
	}

	[RelayCommand]
	private void GoToActivities()
	{
		// _navService.Navigate<Views.Pages.ActivitiesPage>();
	}

	[RelayCommand]
	private void GoToProfile()
	{
		// _navService.Navigate<Views.Pages.UserProfilePage>();
	}

	[RelayCommand]
	private void Logout()
	{
		// czyszczenie sesji
		_dataManager.CurrentUser = null;
		_navService.Navigate<Views.Pages.LoginPage>();
	}
}