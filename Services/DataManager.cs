using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class DataManager
{
	public string UserDirectory { get; }
	public string UserFilePath => Path.Combine(UserDirectory, "userdata.json");
	public string ActivitiesFilePath => Path.Combine(UserDirectory, "activities.json");
	public string MealsFilePath => Path.Combine(UserDirectory, "meals.json");
	public string TodayEntriesPath => Path.Combine(UserDirectory, "today-entries.json");

	public string EntriesDir => Path.Combine(UserDirectory, "entries");
	public string DailiesDir => Path.Combine(UserDirectory, "dailies");
	public string ReportsDir => Path.Combine(UserDirectory, "reports");

	private readonly JsonSerializerOptions _options;

	public DataManager(string userDirectory)
	{
		UserDirectory = userDirectory;

		Directory.CreateDirectory(UserDirectory);
		Directory.CreateDirectory(EntriesDir);
		Directory.CreateDirectory(DailiesDir);
		Directory.CreateDirectory(ReportsDir);

		_options = new JsonSerializerOptions
		{
			WriteIndented = true,
			Converters = { new EntryConverter() }
		};
	}

	// ---------------------------------------------------------
	// USERDATA.JSON
	// ---------------------------------------------------------

	public void SaveUserData(User user)
	{
		File.WriteAllText(UserFilePath, JsonSerializer.Serialize(user, _options));
	}

	public User LoadUserData()
	{
		if (!File.Exists(UserFilePath))
			return new User();

		return JsonSerializer.Deserialize<User>(File.ReadAllText(UserFilePath), _options)
			   ?? new User();
	}

	// ---------------------------------------------------------
	// ACTIVITIES.JSON
	// ---------------------------------------------------------

	public void SaveActivities(ActivityCatalog catalog)
	{
		File.WriteAllText(ActivitiesFilePath, JsonSerializer.Serialize(catalog, _options));
	}

	public ActivityCatalog LoadActivities()
	{
		if (!File.Exists(ActivitiesFilePath))
			return new ActivityCatalog();

		return JsonSerializer.Deserialize<ActivityCatalog>(File.ReadAllText(ActivitiesFilePath), _options)
			   ?? new ActivityCatalog();
	}

	// ---------------------------------------------------------
	// MEALS.JSON
	// ---------------------------------------------------------

	public void SaveMeals(MealCatalog catalog)
	{
		File.WriteAllText(MealsFilePath, JsonSerializer.Serialize(catalog, _options));
	}

	public MealCatalog LoadMeals()
	{
		if (!File.Exists(MealsFilePath))
			return new MealCatalog();

		return JsonSerializer.Deserialize<MealCatalog>(File.ReadAllText(MealsFilePath), _options)
			   ?? new MealCatalog();
	}

	// ---------------------------------------------------------
	// TODAY-ENTRIES.JSON
	// ---------------------------------------------------------

	public void SaveTodayEntries(List<Entry> entries)
	{
		var wrapper = new TodayEntriesWrapper
		{
			Date = DateTime.Today,
			Entries = entries
		};

		File.WriteAllText(TodayEntriesPath, JsonSerializer.Serialize(wrapper, _options));
	}

	public List<Entry> LoadTodayEntries()
	{
		if (!File.Exists(TodayEntriesPath))
		{
			SaveTodayEntries(new List<Entry>());
			return new List<Entry>();
		}

		var wrapper = JsonSerializer.Deserialize<TodayEntriesWrapper>(File.ReadAllText(TodayEntriesPath), _options);

		if (wrapper == null || wrapper.Date.Date != DateTime.Today)
		{
			// przeniesienie starego pliku do entries/
			string oldPath = Path.Combine(EntriesDir, $"entries-{wrapper?.Date:yyyy-MM-dd}.json");
			File.WriteAllText(oldPath, JsonSerializer.Serialize(wrapper, _options));

			// reset today-entries.json
			SaveTodayEntries(new List<Entry>());
			return new List<Entry>();
		}

		return wrapper.Entries ?? new List<Entry>();
	}

	// ---------------------------------------------------------
	// ENTRIES/YYYY-MM-DD.JSON
	// ---------------------------------------------------------

	public void SaveEntries(List<Entry> entries, DateTime date)
	{
		string path = Path.Combine(EntriesDir, $"entries-{date:yyyy-MM-dd}.json");

		var wrapper = new TodayEntriesWrapper
		{
			Date = date,
			Entries = entries
		};

		File.WriteAllText(path, JsonSerializer.Serialize(wrapper, _options));
	}

	public List<Entry> LoadEntries(DateTime date)
	{
		string path = Path.Combine(EntriesDir, $"entries-{date:yyyy-MM-dd}.json");

		if (!File.Exists(path))
			return new List<Entry>();

		var wrapper = JsonSerializer.Deserialize<TodayEntriesWrapper>(File.ReadAllText(path), _options);
		return wrapper?.Entries ?? new List<Entry>();
	}

	// ---------------------------------------------------------
	// DAILIES/YYYY-MM-DD.JSON
	// ---------------------------------------------------------

	public void SaveDailySummary(DailySummary summary)
	{
		string path = Path.Combine(DailiesDir, $"daily-{summary.Date:yyyy-MM-dd}.json");
		File.WriteAllText(path, JsonSerializer.Serialize(summary, _options));
	}

	public DailySummary LoadDailySummary(DateTime date)
	{
		string path = Path.Combine(DailiesDir, $"daily-{date:yyyy-MM-dd}.json");

		if (!File.Exists(path))
			return new DailySummary { Date = date };

		return JsonSerializer.Deserialize<DailySummary>(File.ReadAllText(path), _options)
			   ?? new DailySummary { Date = date };
	}

	// ---------------------------------------------------------
	// REPORTS/YYYYMMDD-YYYYMMDD.JSON
	// ---------------------------------------------------------

	public void SaveReport(Report report)
	{
		string path = Path.Combine(ReportsDir,
			$"report-{report.StartDate:yyyyMMdd}-{report.EndDate:yyyyMMdd}.json");

		File.WriteAllText(path, JsonSerializer.Serialize(report, _options));
	}

	public Report LoadReport(DateTime start, DateTime end)
	{
		string path = Path.Combine(ReportsDir,
			$"report-{start:yyyyMMdd}-{end:yyyyMMdd}.json");

		if (!File.Exists(path))
			return new Report { StartDate = start, EndDate = end };

		return JsonSerializer.Deserialize<Report>(File.ReadAllText(path), _options)
			   ?? new Report { StartDate = start, EndDate = end };
	}
}

public class TodayEntriesWrapper
{
	public DateTime Date { get; set; }
	public List<Entry> Entries { get; set; } = new();
}
