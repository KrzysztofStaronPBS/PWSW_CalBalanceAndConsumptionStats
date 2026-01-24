using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using PWSW_CalBalanceAndConsumptionStats.Models;
using PWSW_CalBalanceAndConsumptionStats.Catalogs;

namespace PWSW_CalBalanceAndConsumptionStats.Services;

public class DataManager
{
	private readonly string _appRoot;
	private readonly JsonSerializerSettings _settings;

	public User? CurrentUser { get; set; }

	// dynamiczne ścieżki oparte o zalogowanego użytkownika
	private string UserRoot => CurrentUser != null ? Path.Combine(_appRoot, CurrentUser.Name) : string.Empty;

	public string UserFilePath => Path.Combine(UserRoot, "userdata.json");
	public string MealTemplatesPath => Path.Combine(UserRoot, "meals.json");
	public string ActivityTemplatesPath => Path.Combine(UserRoot, "activities.json");
	public string TodayEntriesPath => Path.Combine(UserRoot, "today-entries.json");

	public string EntriesDir => Path.Combine(UserRoot, "entries");
	public string DailiesDir => Path.Combine(UserRoot, "dailies");
	public string ReportsDir => Path.Combine(UserRoot, "reports");

	public bool DoesUserExist(string username)
	{
		string userFolder = Path.Combine(_appRoot, username);
		string userFile = Path.Combine(userFolder, "userdata.json");
		return Directory.Exists(userFolder) && File.Exists(userFile);
	}

	public DataManager(string appRoot)
	{
		_appRoot = appRoot;
		Directory.CreateDirectory(_appRoot);

		_settings = new JsonSerializerSettings
		{
			Formatting = Formatting.Indented,
			TypeNameHandling = TypeNameHandling.Auto,
			NullValueHandling = NullValueHandling.Ignore,
			DateFormatString = "yyyy-MM-ddTHH:mm:ss"
		};
	}



	// user data

	public void InitializeUserStructure(string username, User userData)
	{
		string path = Path.Combine(_appRoot, username);

		// tworzenie katalogów głównych i podkatalogów
		Directory.CreateDirectory(path);
		Directory.CreateDirectory(Path.Combine(path, "entries"));
		Directory.CreateDirectory(Path.Combine(path, "dailies"));
		Directory.CreateDirectory(Path.Combine(path, "reports"));

		// inicjalizacja pustych plików JSON (puste listy [])
		string emptyList = JsonConvert.SerializeObject(new List<object>(), _settings);

		File.WriteAllText(Path.Combine(path, "meals.json"), emptyList);
		File.WriteAllText(Path.Combine(path, "activities.json"), emptyList);
		File.WriteAllText(Path.Combine(path, "today-entries.json"), emptyList);

		// zapis danych profilu (userdata.json)
		string userJson = JsonConvert.SerializeObject(userData, _settings);
		File.WriteAllText(Path.Combine(path, "userdata.json"), userJson);

		// ustawienie kontekstu dla aktualnej sesji
		CurrentUser = userData;
	}

	public bool TryAutoLogin()
	{
		if (!Directory.Exists(_appRoot)) return false;

		var userFolders = Directory.GetDirectories(_appRoot);

		if (userFolders.Length == 1)
		{
			// jeśli jest tylko jeden użytkownik, zaloguj go automatycznie
			string username = Path.GetFileName(userFolders[0]);
			return Login(username);
		}
		return false;
	}

	public bool Login(string username)
	{
		string potentialPath = Path.Combine(_appRoot, username, "userdata.json");

		if (File.Exists(potentialPath))
		{
			try
			{
				string json = File.ReadAllText(potentialPath);
				CurrentUser = JsonConvert.DeserializeObject<User>(json, _settings);
				return CurrentUser != null;
			}
			catch { return false; }
		}
		return false;
	}

	public User? LoadSpecificUser(string username)
	{
		if (!DoesUserExist(username)) return null;

		try
		{
			string path = Path.Combine(_appRoot, username, "userdata.json");
			string json = File.ReadAllText(path);
			return JsonConvert.DeserializeObject<User>(json, _settings);
		}
		catch (JsonException)
		{
			return null;
		}
	}

	public void SaveUserData(User user)
	{
		if (string.IsNullOrEmpty(UserRoot)) return;
		File.WriteAllText(UserFilePath, JsonConvert.SerializeObject(user, _settings));
	}

	public User LoadUserData()
	{
		if (!File.Exists(UserFilePath)) return new User();
		try
		{
			string json = File.ReadAllText(UserFilePath);
			return JsonConvert.DeserializeObject<User>(json, _settings) ?? new User();
		}
		catch (JsonException) { return new User(); }
	}

	public void DeleteAccount()
	{
		if (CurrentUser == null || !Directory.Exists(UserRoot)) return;
		Directory.Delete(UserRoot, true);
		CurrentUser = null;
	}



	// entries

	public void SaveEntries(DateTime date, List<Entry> entries)
	{
		string fileName = $"entries-{date:yyyy-MM-dd}.json";
		string path = Path.Combine(EntriesDir, fileName);

		string json = JsonConvert.SerializeObject(entries, _settings);
		File.WriteAllText(path, json);
	}

	public List<Entry> LoadEntries(DateTime date)
	{
		string fileName = $"entries-{date:yyyy-MM-dd}.json";
		string path = Path.Combine(EntriesDir, fileName);

		if (!File.Exists(path)) return new List<Entry>();

		try
		{
			string json = File.ReadAllText(path);
			return JsonConvert.DeserializeObject<List<Entry>>(json, _settings) ?? new List<Entry>();
		}
		catch (JsonException)
		{
			return new List<Entry>();
		}
	}

	public List<Entry> GetAllEntries()
	{
		var allEntries = new List<Entry>();

		// dodawanie wpisów z dzisiaj (today-entries.json)
		if (File.Exists(TodayEntriesPath))
		{
			try
			{
				string json = File.ReadAllText(TodayEntriesPath);
				var today = JsonConvert.DeserializeObject<List<Entry>>(json, _settings);
				if (today != null) allEntries.AddRange(today);
			}
			catch { /* log error */ }
		}

		// 2. dodawanie wpisów z archiwum (/entries/entries-*.json)
		if (Directory.Exists(EntriesDir))
		{
			var files = Directory.GetFiles(EntriesDir, "entries-*.json");
			foreach (var file in files)
			{
				try
				{
					string json = File.ReadAllText(file);
					var archived = JsonConvert.DeserializeObject<List<Entry>>(json, _settings);
					if (archived != null) allEntries.AddRange(archived);
				}
				catch { continue; }
			}
		}

		return allEntries;
	}

	public List<Entry> GetTodayEntries()
	{
		if (!File.Exists(TodayEntriesPath)) return new List<Entry>();
		try
		{
			string json = File.ReadAllText(TodayEntriesPath);
			return JsonConvert.DeserializeObject<List<Entry>>(json, _settings) ?? new List<Entry>();
		}
		catch { return new List<Entry>(); }
	}

	public void AddEntryToToday(Entry entry)
	{
		if (CurrentUser == null) throw new InvalidOperationException("Sesja nieaktywna.");

		var entries = GetTodayEntries();

		entry.Id = entries.Any() ? entries.Max(e => e.Id) + 1 : 1;

		entry.DateTime = DateTime.Now;

		entries.Add(entry);

		string json = JsonConvert.SerializeObject(entries, _settings);
		File.WriteAllText(TodayEntriesPath, json);
	}

	public void ArchiveTodayEntries()
	{
		if (!File.Exists(TodayEntriesPath)) return;

		var entries = GetTodayEntries();
		if (!entries.Any()) return;

		// pobranie daty z pierwszego wpisu (pole Date) lub dzisiejszą
		DateTime archiveDate = entries.FirstOrDefault()?.DateTime.Date ?? DateTime.Today;

		// zapis do pliku entries-yyyy-MM-dd.json w folderze /entries/
		SaveEntries(archiveDate, entries);

		// czyszczenie pliku dzisiejszego
		File.WriteAllText(TodayEntriesPath, JsonConvert.SerializeObject(new List<Entry>(), _settings));
	}



	// meals

	public List<MealTemplate> GetMealTemplates()
	{
		if (!File.Exists(MealTemplatesPath)) return new List<MealTemplate>();
		try
		{
			string json = File.ReadAllText(MealTemplatesPath);
			return JsonConvert.DeserializeObject<List<MealTemplate>>(json, _settings) ?? new List<MealTemplate>();
		}
		catch { return new List<MealTemplate>(); }
	}

	public void AddMealTemplate(MealTemplate template)
	{
		if (CurrentUser == null) throw new InvalidOperationException("Brak zalogowanego użytkownika.");

		var templates = GetMealTemplates();

		template.Id = templates.Any() ? templates.Max(m => m.Id) + 1 : 1;

		templates.Add(template);

		string json = JsonConvert.SerializeObject(templates, _settings);
		File.WriteAllText(MealTemplatesPath, json);
	}


	// activities

	public List<ActivityTemplate> GetActivityTemplates()
	{
		if (!File.Exists(ActivityTemplatesPath)) return new List<ActivityTemplate>();
		try
		{
			string json = File.ReadAllText(ActivityTemplatesPath);
			return JsonConvert.DeserializeObject<List<ActivityTemplate>>(json, _settings) ?? new List<ActivityTemplate>();
		}
		catch { return new List<ActivityTemplate>(); }
	}

	public void AddActivityTemplate(ActivityTemplate template)
	{
		if (CurrentUser == null) throw new InvalidOperationException("Brak zalogowanego użytkownika.");

		var templates = GetActivityTemplates();

		template.Id = templates.Any() ? templates.Max(a => a.Id) + 1 : 1;

		templates.Add(template);

		string json = JsonConvert.SerializeObject(templates, _settings);
		File.WriteAllText(ActivityTemplatesPath, json);
	}

	// daily summary

	public void SaveDailySummary(DailySummary summary)
	{
		string fileName = $"daily-{summary.Date:yyyy-MM-dd}.json";
		string path = Path.Combine(DailiesDir, fileName);

		string json = JsonConvert.SerializeObject(summary, _settings);
		File.WriteAllText(path, json);
	}

	public void RecalculateDailySummary(DateTime date)
	{
		// pobieranie surowych wpisów dla danego dnia
		var entries = LoadEntries(date);

		// obliczanie statystyk
		double totalEaten = entries.OfType<Meal>().Sum(m => m.Calories);
		double totalBurned = entries.OfType<Activity>().Sum(a => a.Calories);

		// 3. tworzenie obiektu podsumowania
		var summary = new DailySummary
		{
			Date = date,
			TotalEaten = totalEaten,
			TotalBurned = totalBurned
		};

		// zapisanie / nadpisanie pliku w folderze dailies
		string fileName = $"daily-{date:yyyy-MM-dd}.json";
		string path = Path.Combine(DailiesDir, fileName);

		string json = JsonConvert.SerializeObject(summary, _settings);
		File.WriteAllText(path, json);
	}
}