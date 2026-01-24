using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PWSW_CalBalanceAndConsumptionStats.Models;
using PWSW_CalBalanceAndConsumptionStats.Services;
using PWSW_CalBalanceAndConsumptionStats.Views.Pages;

namespace PWSW_CalBalanceAndConsumptionStats.ViewModels;

public partial class MainViewModel : ObservableObject
{
	private readonly NavigationService _navService;
	private readonly DataManager _dataManager;

	[ObservableProperty] private string _welcomeMessage = "Witaj!";

	// Właściwości dla Dashboardu
	[ObservableProperty] private double _totalEaten;
	[ObservableProperty] private double _totalBurned;
	[ObservableProperty] private double _remaining;
	[ObservableProperty] private double _progressValue;
	[ObservableProperty] private string _progressText = "Obliczanie...";

	public MainViewModel(NavigationService navService, DataManager dataManager)
	{
		_navService = navService;
		_dataManager = dataManager;

		if (_dataManager.CurrentUser is not null)
		{
			WelcomeMessage = $"Witaj, {_dataManager.CurrentUser.Name}!";
		}

		UpdateDashboard();
	}

	public void UpdateDashboard()
	{
		var user = _dataManager.CurrentUser;
		if (user == null) return;

		double goal = user.CalculatedDailyGoal;

		var todayEntries = _dataManager.GetTodayEntries();
		TotalEaten = todayEntries.Where(e => e.Type == EntryType.Meal).Sum(e => e.Calories);
		TotalBurned = todayEntries.Where(e => e.Type == EntryType.Activity).Sum(e => e.Calories);

		// formuła: Limit Dnia + Bonus za ruch - Jedzenie
		Remaining = goal + TotalBurned - TotalEaten;

		double targetTotal = goal + TotalBurned;
		ProgressValue = targetTotal > 0 ? (TotalEaten / targetTotal) * 100 : 0;
		if (ProgressValue > 100) ProgressValue = 100;

		ProgressText = $"Limit bazowy: {goal:F0} kcal | Aktywność: +{TotalBurned:F0} kcal";
	}

	[RelayCommand] private void ShowEntries() => _navService.Navigate<EntriesPage>();
	[RelayCommand] private void NavigateToCatalog() => _navService.Navigate<CatalogEditorPage>();
	[RelayCommand] private void GenerateReport() { _navService.Navigate<ReportsPage>(); }
	[RelayCommand] private void EditProfile() => _navService.Navigate<ProfilePage>();

	[RelayCommand]
	private void Logout()
	{
		_dataManager.CurrentUser = null;
		_navService.Navigate<LoginPage>();
	}
}