using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Collections.Generic;

public class ActivityCatalog
{
    public List<ActivityTemplate> Activities { get; set; } = new();

    public void AddTemplate(ActivityTemplate template)
    {
        Activities.Add(template);
    }

    public double GetMet(string name)
    {
        return Activities.FirstOrDefault(a => a.Name == name)?.MET
            ?? throw new Exception("Activity not found.");
    }

    public static ActivityCatalog Load(string filePath)
    {
        if (!File.Exists(filePath))
            return new ActivityCatalog();

        string json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<ActivityCatalog>(json) ?? new ActivityCatalog();
    }

    public void Save(string filePath)
    {
        string json = JsonSerializer.Serialize(this,
            new JsonSerializerOptions { WriteIndented = true });

        File.WriteAllText(filePath, json);
    }
}
