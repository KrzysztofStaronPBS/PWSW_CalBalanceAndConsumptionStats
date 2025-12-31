using System;

public enum EntryType
{
    Meal,
    Activity
}

public abstract class Entry
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Calories { get; set; }
    public DateTime Date { get; set; }

    public abstract EntryType Type { get; }

    public virtual void Validate()
    {
        if (Id < 1)
            throw new InvalidEntryException("Id must be >= 1.");

        if (string.IsNullOrWhiteSpace(Name) || Name.Length > 50)
            throw new InvalidEntryException("Name must be non-empty and max 50 characters.");

        if (Calories < 0)
            throw new InvalidEntryException("Calories must be >= 0.");

        if (Date > DateTime.Now)
            throw new InvalidEntryException("Date cannot be in the future.");
    }
	public override string ToString()
	{
		return $"{Type}: {Name} ({Calories} kcal) on {Date:yyyy-MM-dd}";
	}

}

public class InvalidEntryException : Exception
{
    public InvalidEntryException(string message) : base(message) { }
}
