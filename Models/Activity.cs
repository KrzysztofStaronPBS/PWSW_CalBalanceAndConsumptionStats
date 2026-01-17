namespace PWSW_CalBalanceAndConsumptionStats.Models;

public class Activity : Entry
{
	public int Duration { get; set; }
	public double MET { get; set; }
	public override EntryType Type => EntryType.Activity;

	public void CalculateCalories(User user)
	{
		// formuła: kcal = 0.0175 * MET * waga_kg * czas_min
		Calories = 0.0175 * MET * user.Weight * Duration;
	}

	public override void Validate()
	{
		base.Validate();
		if (Duration <= 0) throw new InvalidEntryException("Czas trwania musi być dodatni.");
		if (MET <= 0) throw new InvalidEntryException("Wartość MET musi być dodatnia.");
	}
}