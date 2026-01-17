using System;
using PWSW_CalBalanceAndConsumptionStats.Models;

namespace PWSW_CalBalanceAndConsumptionStats.Services;

public class ReportGenerator
{
	private readonly DataManager _data;

	public ReportGenerator(DataManager dataManager)
	{
		_data = dataManager;
	}

	public Report GenerateReport(DateTime start, DateTime end)
	{
		var report = new Report
		{
			Title = $"Raport {start:yyyy-MM-dd} - {end:yyyy-MM-dd}",
			StartDate = start,
			EndDate = end
		};

		for (var date = start.Date; date <= end.Date; date = date.AddDays(1))
		{
			var entries = _data.LoadEntries(date);
			var summary = new DailySummary { Date = date };
			summary.Calculate(entries);
			report.Summaries.Add(summary);
		}

		return report;
	}
}