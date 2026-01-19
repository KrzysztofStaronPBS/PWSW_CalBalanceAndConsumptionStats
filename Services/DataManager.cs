using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using PWSW_CalBalanceAndConsumptionStats.Models;

namespace PWSW_CalBalanceAndConsumptionStats.Services;

public class DataManager
{
	private readonly string _baseDir;
	private readonly JsonSerializerSettings _settings;

	public string UserFilePath => Path.Combine(_baseDir, "userdata.json");
	public string EntriesDir => Path.Combine(_baseDir, "entries");
	public string ReportsDir => Path.Combine(_baseDir, "reports");

	public bool HasUserData => File.Exists(UserFilePath);
	public User? CurrentUser { get; set; }

	public DataManager(string baseDir)
	{
		_baseDir = baseDir;

		Directory.CreateDirectory(_baseDir);
		Directory.CreateDirectory(EntriesDir);
		Directory.CreateDirectory(ReportsDir);

		_settings = new JsonSerializerSettings
		{
			Formatting = Formatting.Indented,
			TypeNameHandling = TypeNameHandling.Auto,
			NullValueHandling = NullValueHandling.Ignore,
			DateFormatString = "yyyy-MM-dd"
		};
	}



	// user data

	public void SaveUserData(User user)
	{
		string json = JsonConvert.SerializeObject(user, _settings);
		File.WriteAllText(UserFilePath, json);
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



	// zarządzanie kontem

	public void DeleteAccount()
	{
		// usunięcie głównego pliku usera
		if (File.Exists(UserFilePath)) File.Delete(UserFilePath);

		// rekurencyjne usunięcie danych
		if (Directory.Exists(EntriesDir)) Directory.Delete(EntriesDir, true);
		if (Directory.Exists(ReportsDir)) Directory.Delete(ReportsDir, true);

		// re-inicjalizacja pustych katalogów
		Directory.CreateDirectory(EntriesDir);
		Directory.CreateDirectory(ReportsDir);

		CurrentUser = null;
	}
}