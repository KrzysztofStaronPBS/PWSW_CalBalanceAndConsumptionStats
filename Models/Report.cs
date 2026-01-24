using System;
using System.Collections.Generic;

namespace PWSW_CalBalanceAndConsumptionStats.Models;

public class Report
{
	public string Title { get; set; } = string.Empty;
	public DateTime StartDate { get; set; }
	public DateTime EndDate { get; set; }
	public List<DailySummary> Summaries { get; set; } = [];
}