using System;
using System.Linq;

public class DailySummary
{
	public DateTime Date { get; set; }
	public int TotalEaten { get; private set; }
	public int TotalBurned { get; private set; }
	public int NetCalories => TotalEaten - TotalBurned;

	public void Calculate(User user)
	{
		var entries = user.Entries.Where(e => e.Date.Date == Date.Date);
		TotalEaten = entries.OfType<Meal>().Sum(m => m.Calories);
		TotalBurned = entries.OfType<Activity>().Sum(a => a.Calories);
	}
}
