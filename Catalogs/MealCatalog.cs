using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;

namespace PWSW_CalBalanceAndConsumptionStats.Catalogs;

public class MealCatalog
{
	// ObservableCollection dla automatycznego powiadamiania UI
	public ObservableCollection<MealTemplate> Meals { get; set; } = new();

	public void AddTemplate(MealTemplate template)
	{
		if (string.IsNullOrWhiteSpace(template.Name))
			throw new ArgumentException("Nazwa posiłku nie może być pusta.");
		if (template.CaloriesPerPortion <= 0)
			throw new ArgumentException("Liczba kalorii musi być większa od 0.");
		if (Meals.Any(m => m.Name.Equals(template.Name, StringComparison.OrdinalIgnoreCase)))
			throw new InvalidOperationException("Taki posiłek już istnieje w katalogu.");

		Meals.Add(template);
	}

	public double GetCalories(string name)
	{
		var meal = Meals.FirstOrDefault(m => m.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
		return meal?.CaloriesPerPortion ?? throw new KeyNotFoundException($"Nie znaleziono posiłku: {name}");
	}
}