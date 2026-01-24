using System;

namespace PWSW_CalBalanceAndConsumptionStats.Models;

public class ReportDay
{
	public DateTime Date { get; set; }
	public double Eaten { get; set; }
	public double Burned { get; set; }
	public double Balance => Eaten - Burned;
	public string Status => Balance > 0 ? "Nadwyżka" : "Deficyt";
	public string FormattedDate => Date.ToString("dd.MM.yyyy");
}