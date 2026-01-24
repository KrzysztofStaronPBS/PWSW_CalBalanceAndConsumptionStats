using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PWSW_CalBalanceAndConsumptionStats.Models;
using PWSW_CalBalanceAndConsumptionStats.Catalogs;
using PWSW_CalBalanceAndConsumptionStats.Services;

namespace PWSW_CalBalanceAndConsumptionStats.ViewModels;

public partial class AddEntryViewModel : ObservableObject
{
	private readonly DataManager _dataManager;
	private readonly NavigationService _navService;

	[ObservableProperty] private ObservableCollection<MealTemplate> _mealTemplates = new();
	[ObservableProperty] private ObservableCollection<ActivityTemplate> _activityTemplates = new();

	[ObservableProperty] private MealTemplate _selectedMeal;
	[ObservableProperty] private ActivityTemplate _selectedActivity;

	[ObservableProperty] private double _quantity = 1;
	[ObservableProperty] private int _duration = 30;

	public AddEntryViewModel(DataManager dataManager, NavigationService navService)
	{
		_dataManager = dataManager;
		_navService = navService;
		LoadTemplates();
	}

	private void LoadTemplates()
	{
		MealTemplates = new ObservableCollection<MealTemplate>(_dataManager.GetMealTemplates());
		ActivityTemplates = new ObservableCollection<ActivityTemplate>(_dataManager.GetActivityTemplates());
	}

	[RelayCommand]
	private void SaveMeal()
	{
		if (SelectedMeal == null || Quantity <= 0) return;

		var mealEntry = new Meal
		{
			Name = SelectedMeal.Name,
			Unit = SelectedMeal.Unit,
			Quantity = Quantity,
			Calories = SelectedMeal.CaloriesPerPortion * Quantity,
			DateTime = System.DateTime.Now
		};

		_dataManager.AddEntryToToday(mealEntry);
		_navService.Navigate<Views.Pages.EntriesPage>();
	}

	[RelayCommand]
	private void SaveActivity()
	{
		if (SelectedActivity == null || Duration <= 0) return;

		var activityEntry = new Activity
		{
			Name = SelectedActivity.Name,
			MET = SelectedActivity.MET,
			Duration = Duration,
			DateTime = System.DateTime.Now
		};

		// Twoja metoda obliczająca kcal na podstawie wagi usera z userdata.json
		activityEntry.CalculateCalories(_dataManager.CurrentUser);

		_dataManager.AddEntryToToday(activityEntry);
		_navService.Navigate<Views.Pages.EntriesPage>();
	}

	[RelayCommand]
	private void Cancel() => _navService.Navigate<Views.Pages.EntriesPage>();
}