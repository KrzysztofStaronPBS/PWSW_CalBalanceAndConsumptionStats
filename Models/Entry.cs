using System;

namespace PWSW_CalBalanceAndConsumptionStats.Models;

public enum EntryType { Meal, Activity }

// atrybuty JsonConverter pozwolą na poprawne wczytywanie list mieszanych (Meal i Activity)
public abstract class Entry
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public double Calories { get; set; }
	public DateTime Date { get; set; }

	public abstract EntryType Type { get; }

	public virtual void Validate()
	{
		if (Id < 0) throw new InvalidEntryException("Id nie może być ujemne.");
		if (string.IsNullOrWhiteSpace(Name)) throw new InvalidEntryException("Nazwa nie może być pusta.");
		if (Date > DateTime.Now) throw new InvalidEntryException("Data nie może być z przyszłości.");
	}

	public override string ToString() => $"{Type}: {Name} ({Calories:F1} kcal) - {Date:yyyy-MM-dd}";
}

public class InvalidEntryException : Exception
{
	public InvalidEntryException(string message) : base(message) { }
}