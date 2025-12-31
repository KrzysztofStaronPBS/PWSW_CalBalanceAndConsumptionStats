using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Collections.Generic;

public class MealCatalog
{
    public List<MealTemplate> Meals { get; set; } = new();

	public void AddTemplate(MealTemplate template)
	{
		if (string.IsNullOrWhiteSpace(template.Name))
			throw new ArgumentException("Meal name cannot be empty.");
		if (template.CaloriesPerPortion <= 0)
			throw new ArgumentException("Calories must be greater than 0.");
		if (Meals.Any(a => a.Name == template.Name))
			throw new InvalidOperationException("Meal already exists.");


		Meals.Add(template);
	}

	public int GetCalories(string name)
    {
        return Meals.FirstOrDefault(m => m.Name == name)?.CaloriesPerPortion
            ?? throw new KeyNotFoundException("Meal not found.");
    }

    public static MealCatalog Load(string filePath)
    {
        if (!File.Exists(filePath))
            return new MealCatalog();

        string json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<MealCatalog>(json) ?? new MealCatalog();
    }

    public void Save(string filePath)
    {
        string json = JsonSerializer.Serialize(this,
            new JsonSerializerOptions { WriteIndented = true });

        File.WriteAllText(filePath, json);
    }
}
