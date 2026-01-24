using System;
using System.Collections.Generic;
using System.Linq;

namespace PWSW_CalBalanceAndConsumptionStats.Models;

public class DailySummary
{
	public DateTime Date { get; set; }
	public double TotalEaten { get; set; }
	public double TotalBurned { get; set; }
	public double NetCalories => TotalEaten - TotalBurned;

	public void Calculate(IEnumerable<Entry> entries)
	{
		var dayEntries = entries.Where(e => e.DateTime.Date == Date.Date).ToList();
		TotalEaten = dayEntries.OfType<Meal>().Sum(m => m.Calories);
		TotalBurned = dayEntries.OfType<Activity>().Sum(a => a.Calories);
	}
}