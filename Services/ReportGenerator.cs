using System;
using System.Linq;

public class ReportGenerator
{
	public Report GenerateReport(User user, DateTime startDate, DateTime endDate)
	{
		var report = new Report
		{
			Title = $"Raport {startDate:yyyy-MM-dd} - {endDate:yyyy-MM-dd}",
			StartDate = startDate,
			EndDate = endDate
		};

		for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
		{
			var summary = user.GetDailySummary(date);
			report.Summaries.Add(summary);
		}

		return report;
	}

	public object GenerateChartData(User user)
	{
		return user.Entries
			.GroupBy(e => e.Date.Date)
			.Select(g => new { Date = g.Key, Calories = g.Sum(e => e.Calories) })
			.ToList();
	}
}
