namespace PWSW_CalBalanceAndConsumptionStats.Catalogs;

public class MealTemplate
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public double CaloriesPerPortion { get; set; }
	public string DefaultUnit { get; set; } = "szt.";
}