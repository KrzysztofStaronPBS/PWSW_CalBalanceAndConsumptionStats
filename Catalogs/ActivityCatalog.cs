using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;

namespace PWSW_CalBalanceAndConsumptionStats.Catalogs;

public class ActivityCatalog
{
	public ObservableCollection<ActivityTemplate> Activities { get; set; } = new();

	public void AddTemplate(ActivityTemplate template)
	{
		if (string.IsNullOrWhiteSpace(template.Name))
			throw new ArgumentException("Nazwa aktywności nie może być pusta.");
		if (template.MET <= 0)
			throw new ArgumentException("Wartość MET musi być większa od 0.");
		if (Activities.Any(a => a.Name.Equals(template.Name, StringComparison.OrdinalIgnoreCase)))
			throw new InvalidOperationException("Taka aktywność już istnieje w katalogu.");

		Activities.Add(template);
	}

	public double GetMet(string name)
	{
		var activity = Activities.FirstOrDefault(a => a.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
		return activity?.MET ?? throw new KeyNotFoundException($"Nie znaleziono aktywności: {name}");
	}
}