using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PWSW_CalBalanceAndConsumptionStats.Models;
using PWSW_CalBalanceAndConsumptionStats.Services;
using Microsoft.UI.Xaml; // Potrzebne do Visibility
using PWSW_CalBalanceAndConsumptionStats.Catalogs;

namespace PWSW_CalBalanceAndConsumptionStats.ViewModels;

public partial class CatalogEditorViewModel : ObservableObject
{
	private readonly DataManager _dataManager;
	private readonly NavigationService _navService;

	[ObservableProperty] private string _name = string.Empty;
	[ObservableProperty] private double _value;
	[ObservableProperty] private string _unitName = "szt.";

	// Logika sterowania widokiem
	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(IsMealType))]
	[NotifyPropertyChangedFor(nameof(ValueLabel))]
	[NotifyPropertyChangedFor(nameof(NamePlaceholder))]
	[NotifyPropertyChangedFor(nameof(UnitSuffix))]
	private int _selectedTypeIndex = 0;

	public bool IsMealType => SelectedTypeIndex == 0;

	public string NamePlaceholder => IsMealType
	? "np. Owsianka, Kurczak grillowany"
	: "np. Bieganie, Jazda na rowerze";

	public string UnitSuffix => IsMealType ? "kcal" : "met";

	// Dynamiczna etykieta
	public string ValueLabel => SelectedTypeIndex == 0 ? "Kalorie na 1 jednostkę" : "Wartość MET";

	public CatalogEditorViewModel(DataManager dataManager, NavigationService navService)
	{
		_dataManager = dataManager;
		_navService = navService;
	}

	[RelayCommand]
	private void SaveToDatabase()
	{
		if (string.IsNullOrWhiteSpace(Name) || Value <= 0) return;

		if (IsMealType)
		{
			if (string.IsNullOrWhiteSpace(UnitName)) return;

			var newMealTemplate = new MealTemplate
			{
				Name = Name,
				CaloriesPerPortion = Value,
				Unit = UnitName
			};
			_dataManager.AddMealTemplate(newMealTemplate);
		}
		else
		{
			var newActivityTemplate = new ActivityTemplate
			{
				Name = Name,
				MET = Value
			};
			_dataManager.AddActivityTemplate(newActivityTemplate);
		}

		_navService.Navigate<Views.Pages.MainPage>();
	}

	[RelayCommand]
	private void GoBack() => _navService.Navigate<Views.Pages.MainPage>();
}