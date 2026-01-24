using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PWSW_CalBalanceAndConsumptionStats.Services;
using PWSW_CalBalanceAndConsumptionStats.Views.Pages;

namespace PWSW_CalBalanceAndConsumptionStats.ViewModels;

public partial class MainViewModel : ObservableObject
{
	private readonly NavigationService _navService;
	private readonly DataManager _dataManager;

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

	// wyświetl wpisy (Lista wpisów - historia)
	[RelayCommand]
	private void ShowEntries()
	{
		 _navService.Navigate<PWSW_CalBalanceAndConsumptionStats.Views.Pages.EntriesPage>();
	}

	// dodaj nowy posiłek lub aktywność (Rozbudowa bazy danych/Katalogu)
	[RelayCommand]
	private void NavigateToCatalog()
	{
		_navService.Navigate<CatalogEditorPage>();
	}

	// generuj raport
	[RelayCommand]
	private void GenerateReport()
	{
		// _navService.Navigate<ReportsPage>();
	}

	// podsumowanie dnia (Licznik kalorii, wykresy dzienne)
	[RelayCommand]
	private void ShowDailySummary()
	{
		// _navService.Navigate<DailySummaryPage>();
	}

	// profil usera
	[RelayCommand]
	private void EditProfile()
	{
		_navService.Navigate<PWSW_CalBalanceAndConsumptionStats.Views.Pages.ProfilePage>();
	}

	[RelayCommand]
	private void Logout()
	{
		_dataManager.CurrentUser = null;
		_navService.Navigate<PWSW_CalBalanceAndConsumptionStats.Views.Pages.LoginPage>();
	}
}