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
			DateFormatString = "yyyy-MM-dd"
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

		if (!Directory.Exists(EntriesDir)) return allEntries;

		// pobieranie wszystkich plików pasujących do wzorca
		string[] files = Directory.GetFiles(EntriesDir, "entries-*.json");

		foreach (var file in files)
		{
			try
			{
				string json = File.ReadAllText(file);
				var dayEntries = JsonConvert.DeserializeObject<List<Entry>>(json, _settings);
				if (dayEntries != null)
				{
					allEntries.AddRange(dayEntries);
				}
			}
			catch (JsonException)
			{
				continue;
			}
		}

		return allEntries;
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



	// zarządzanie kontem

	public void DeleteAccount()
	{
		if (CurrentUser == null || !Directory.Exists(UserRoot)) return;
		Directory.Delete(UserRoot, true);
		CurrentUser = null;
	}
}