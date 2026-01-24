using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using PWSW_CalBalanceAndConsumptionStats.Models;
using PWSW_CalBalanceAndConsumptionStats.Services;
using PWSW_CalBalanceAndConsumptionStats.Views.Pages;

namespace PWSW_CalBalanceAndConsumptionStats.ViewModels;

public partial class ReportsViewModel : ObservableObject
{
	private readonly DataManager _dataManager;
	private readonly NavigationService _navService;
	[ObservableProperty] private DateTimeOffset _startDate = DateTimeOffset.Now.AddDays(-7);
	[ObservableProperty] private DateTimeOffset _endDate = DateTimeOffset.Now;
	public ObservableCollection<ReportDay> ReportEntries { get; } = new();

	public ReportsViewModel(DataManager dataManager, NavigationService navService)
	{
		_dataManager = dataManager;
		_navService = navService;
	}

	[RelayCommand]
	public void GenerateReport()
	{
		ReportEntries.Clear();
		var current = StartDate.Date;

		while (current <= EndDate.Date)
		{
			string fileName = $"daily-{current:yyyy-MM-dd}.json";
			string path = Path.Combine(_dataManager.DailiesDir, fileName);

			if (File.Exists(path))
			{
				var json = File.ReadAllText(path);

				var dayData = JsonConvert.DeserializeObject<dynamic>(json);

				if (dayData != null)
				{
					ReportEntries.Add(new ReportDay
					{
						Date = current,
						Eaten = (double)(dayData.TotalEaten ?? 0),
						Burned = (double)(dayData.TotalBurned ?? 0)
					});
				}
			}
			current = current.AddDays(1);
		}
	}

	[RelayCommand]
	public async Task ExportToCSV(Microsoft.UI.Xaml.XamlRoot xamlRoot)
	{
		if (ReportEntries.Count == 0) return;

		var csv = new StringBuilder();
		csv.AppendLine("Data;Zjedzone;Spalone;Bilans;Status");

		foreach (var entry in ReportEntries)
		{
			csv.AppendLine($"{entry.Date:yyyy-MM-dd};{entry.Eaten};{entry.Burned};{entry.Balance};{entry.Status}");
		}

		string fileName = $"report-{StartDate:yyyyMMdd}-{EndDate:yyyyMMdd}.csv";
		string path = Path.Combine(_dataManager.ReportsDir, fileName);

		try
		{
			if (!Directory.Exists(_dataManager.ReportsDir))
				Directory.CreateDirectory(_dataManager.ReportsDir);

			await File.WriteAllTextAsync(path, csv.ToString(), Encoding.UTF8);

			// Tworzenie i wyświetlenie okna dialogowego
			var dialog = new Microsoft.UI.Xaml.Controls.ContentDialog
			{
				Title = "Sukces",
				Content = $"Raport został pomyślnie wygenerowany w lokalizacji:\n{path}",
				CloseButtonText = "OK",
				XamlRoot = xamlRoot // Kluczowe w WinUI 3
			};
			await dialog.ShowAsync();

			// Otwarcie folderu z raportem
			System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
			{
				FileName = _dataManager.ReportsDir,
				UseShellExecute = true
			});
		}
		catch (Exception ex)
		{
			var errorDialog = new Microsoft.UI.Xaml.Controls.ContentDialog
			{
				Title = "Błąd zapisu",
				Content = $"Nie udało się zapisać raportu: {ex.Message}",
				CloseButtonText = "Zamknij",
				XamlRoot = xamlRoot
			};
			await errorDialog.ShowAsync();
		}
	}

	[RelayCommand]
	private void GoBack() => _navService.Navigate<MainPage>();
}