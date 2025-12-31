using System;
using System.Text.Json;
using System.Text.Json.Serialization;

public class EntryConverter : JsonConverter<Entry>
{
	public override Entry Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		using var doc = JsonDocument.ParseValue(ref reader);
		var root = doc.RootElement;

		if (!root.TryGetProperty("Type", out var typeProp))
			throw new JsonException("Missing Type property for Entry.");

		string type = typeProp.GetString() ?? throw new JsonException("Invalid Type value.");

		return type switch
		{
			"Meal" => JsonSerializer.Deserialize<Meal>(root.GetRawText(), options)
					 ?? throw new JsonException("Failed to deserialize Meal."),
			"Activity" => JsonSerializer.Deserialize<Activity>(root.GetRawText(), options)
						 ?? throw new JsonException("Failed to deserialize Activity."),
			_ => throw new JsonException($"Unknown entry type: {type}")
		};
	}

	public override void Write(Utf8JsonWriter writer, Entry value, JsonSerializerOptions options)
	{
		string type = value switch
		{
			Meal => "Meal",
			Activity => "Activity",
			_ => throw new JsonException("Unsupported Entry type.")
		};

		writer.WriteStartObject();
		writer.WriteString("Type", type);

		var json = JsonSerializer.SerializeToElement(value, value.GetType(), options);

		foreach (var prop in json.EnumerateObject())
			prop.WriteTo(writer);

		writer.WriteEndObject();
	}
}
