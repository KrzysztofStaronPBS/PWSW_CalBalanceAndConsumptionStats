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
        Meals.Add(template);
    }

    public int GetCalories(string name)
    {
        return Meals.FirstOrDefault(m => m.Name == name)?.CaloriesPerPortion
            ?? throw new Exception("Meal not found.");
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
