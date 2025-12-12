using System;
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
