using System;
using System.Collections.Generic;
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

	public void Calculate(List<Entry> entries)
	{
		var todays = entries.Where(e => e.Date.Date == Date.Date);
		TotalEaten = todays.OfType<Meal>().Sum(m => m.Calories);
		TotalBurned = todays.OfType<Activity>().Sum(a => a.Calories);
	}

}
