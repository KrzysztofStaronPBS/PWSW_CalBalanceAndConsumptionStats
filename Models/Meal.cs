namespace PWSW_CalBalanceAndConsumptionStats.Models;

public class Meal : Entry
{
	public double Quantity { get; set; }
	public override EntryType Type => EntryType.Meal;

	public override void Validate()
	{
		base.Validate();
		if (Quantity <= 0) throw new InvalidEntryException("Ilość musi być większa od 0.");
	}
}