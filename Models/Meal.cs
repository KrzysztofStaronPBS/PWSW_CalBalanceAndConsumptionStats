namespace PWSW_CalBalanceAndConsumptionStats.Models;

public class Meal : Entry
{
	// ile jednostek zjedzono (np. 1.5)
	public double Quantity { get; set; }

	// nazwa jednostki (np. "szt.", "g", "ml", "kromka", "opakowanie")
	// domyślnie "g" dla kompatybilności wstecznej
	public string Unit { get; set; } = "g";

	public override EntryType Type => EntryType.Meal;

	public override void Validate()
	{
		base.Validate();
		if (Quantity <= 0) throw new InvalidEntryException("Ilość musi być większa od 0.");
		if (string.IsNullOrWhiteSpace(Unit)) throw new InvalidEntryException("Jednostka miary nie może być pusta.");
	}

	public override string ToString() => $"{base.ToString()} ({Quantity} {Unit})";
}