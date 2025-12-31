using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

public class ReportGenerator
{
	private readonly DataManager _data;

	public ReportGenerator(DataManager dataManager)
	{
		_data = dataManager;
	}

	public Report GenerateReport(DateTime startDate, DateTime endDate)
	{
		var report = new Report
		{
			Title = $"Raport {startDate:yyyy-MM-dd} - {endDate:yyyy-MM-dd}",
			StartDate = startDate,
			EndDate = endDate
		};

		for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
		{
			List<Entry> entries = _data.LoadEntries(date);

			var summary = new DailySummary { Date = date };
			summary.Calculate(entries);

			report.Summaries.Add(summary);
		}

		return report;
	}

	public object GenerateChartData()
	{
		var allFiles = Directory.GetFiles(_data.EntriesDir, "entries-*.json");

		var allEntries = new List<Entry>();

		foreach (var file in allFiles)
		{
			var wrapper = JsonSerializer.Deserialize<TodayEntriesWrapper>(
				File.ReadAllText(file),
				new JsonSerializerOptions { Converters = { new EntryConverter() } }
			);

			if (wrapper?.Entries != null)
				allEntries.AddRange(wrapper.Entries);
		}

		return allEntries
			.GroupBy(e => e.Date.Date)
			.Select(g => new { Date = g.Key, Calories = g.Sum(e => e.Calories) })
			.OrderBy(x => x.Date)
			.ToList();
	}
}
