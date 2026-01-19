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

	// dodaj wpis (logowanie konsumpcji/sportu z dzisiaj)
	// to tutaj użytkownik wybierze z listy co zjadł lub wykonał ćwiczenie i to zapisze.
	[RelayCommand]
	private void AddEntry()
	{
		// _navService.Navigate<AddEntryPage>();
	}

	// dodaj nowy posiłek lub aktywność (Rozbudowa bazy danych/Katalogu)
	[RelayCommand]
	private void AddNewMeal()
	{
		// _navService.Navigate<MealsCatalogPage>(); 
		// lub MealsPage z parametrem trybu edycji
	}

	// dodaj nową aktywność fizyczną (Rozbudowa bazy danych/Katalogu)
	[RelayCommand]
	private void AddNewActivity()
	{
		// _navService.Navigate<ActivitiesCatalogPage>();
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