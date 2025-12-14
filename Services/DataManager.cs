using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class DataManager
{
	public string FilePath { get; set; } = "user.json";

	public void SaveData(User user)
	{
		var options = new JsonSerializerOptions { WriteIndented = true, Converters = { new EntryConverter() } };
		File.WriteAllText(FilePath, JsonSerializer.Serialize(user, options));
	}

	public User LoadData()
	{
		if (!File.Exists(FilePath)) return new User();
		var options = new JsonSerializerOptions { Converters = { new EntryConverter() } };
		return JsonSerializer.Deserialize<User>(File.ReadAllText(FilePath), options) ?? new User();
	}

	public void SaveTodayEntries(User user)
	{
		var wrapper = new { Date = DateTime.Today, Entries = user.Entries };
		var options = new JsonSerializerOptions { WriteIndented = true, Converters = { new EntryConverter() } };
		File.WriteAllText("today-entries.json", JsonSerializer.Serialize(wrapper, options));
	}

	public List<Entry> LoadTodayEntries()
	{
		if (!File.Exists("today-entries.json")) return new List<Entry>();
		var options = new JsonSerializerOptions { Converters = { new EntryConverter() } };
		var wrapper = JsonSerializer.Deserialize<TodayEntriesWrapper>(File.ReadAllText("today-entries.json"), options);
		if (wrapper == null || wrapper.Date.Date != DateTime.Today)
		{
			// reset pliku jeśli data nieaktualna
			File.WriteAllText("today-entries.json", JsonSerializer.Serialize(new { Date = DateTime.Today, Entries = new List<Entry>() }, options));
			return new List<Entry>();
		}
		return wrapper.Entries ?? new List<Entry>();
	}

	public void SaveDailySummary(DailySummary summary, int userId)
	{
		var path = $"daily-{summary.Date:yyyy-MM-dd}.json";
		File.WriteAllText(path, JsonSerializer.Serialize(summary));
	}

	public DailySummary LoadDailySummary(DateTime date, int userId)
	{
		var path = $"daily-{date:yyyy-MM-dd}.json";
		return File.Exists(path) ? JsonSerializer.Deserialize<DailySummary>(File.ReadAllText(path)) ?? new DailySummary { Date = date } : new DailySummary { Date = date };
	}

	public void SaveReport(Report report, int userId)
	{
		var path = $"report-{report.StartDate:yyyyMMdd}-{report.EndDate:yyyyMMdd}.json";
		File.WriteAllText(path, JsonSerializer.Serialize(report));
	}

	public Report LoadReport(DateTime startDate, DateTime endDate, int userId)
	{
		var path = $"report-{startDate:yyyyMMdd}-{endDate:yyyyMMdd}.json";
		return File.Exists(path) ? JsonSerializer.Deserialize<Report>(File.ReadAllText(path)) ?? new Report() : new Report();
	}
}

public class TodayEntriesWrapper
{
	public DateTime Date { get; set; }
	public List<Entry> Entries { get; set; } = new();
}