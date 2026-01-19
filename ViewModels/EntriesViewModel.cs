using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PWSW_CalBalanceAndConsumptionStats.Models;
using PWSW_CalBalanceAndConsumptionStats.Services;

namespace PWSW_CalBalanceAndConsumptionStats.ViewModels;

public partial class EntriesViewModel : ObservableObject
{
	private readonly DataManager _dataManager;
	private readonly NavigationService _navService;
	private List<Entry> _allEntries = new();

	[ObservableProperty] private ObservableCollection<Entry> _filteredEntries = new();
	[ObservableProperty] private DateTimeOffset? _selectedDateFilter;
	[ObservableProperty] private int _selectedTypeIndex = 0;

	public EntriesViewModel(DataManager dataManager, NavigationService navService)
	{
		_dataManager = dataManager;
		_navService = navService;
		LoadData();
	}

	[RelayCommand]
	public void LoadData()
	{
		_allEntries = _dataManager.GetAllEntries()
								  .OrderByDescending(e => e.Date)
								  .ToList();
		ApplyFilters();
	}

	[RelayCommand]
	private void ApplyFilters()
	{
		var query = _allEntries.AsEnumerable();

		if (SelectedDateFilter.HasValue)
		{
			var filterDate = SelectedDateFilter.Value.DateTime.Date;
			query = query.Where(e => e.Date.Date == filterDate);
		}

		if (SelectedTypeIndex == 1) query = query.Where(e => e is Meal);
		if (SelectedTypeIndex == 2) query = query.Where(e => e is Activity);

		FilteredEntries.Clear();
		foreach (var entry in query)
		{
			FilteredEntries.Add(entry);
		}
	}

	[RelayCommand]
	private void ResetFilters()
	{
		SelectedDateFilter = null;
		SelectedTypeIndex = 0;
		ApplyFilters();
	}

	[RelayCommand]
	private void GoBack() => _navService.Navigate<Views.Pages.MainPage>();
}