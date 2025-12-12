using System;
using System.Text.Json;
using System.Text.Json.Serialization;

public class EntryConverter : JsonConverter<Entry>
{
	public override Entry Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		using (var doc = JsonDocument.ParseValue(ref reader))
		{
			var root = doc.RootElement;

			// Odczyt typu wpisu
			if (!root.TryGetProperty("Type", out var typeProp))
				throw new JsonException("Missing Type property for Entry.");

			string type = typeProp.GetString() ?? "";

			Entry entry;
			switch (type)
			{
				case "Meal":
					entry = JsonSerializer.Deserialize<Meal>(root.GetRawText(), options)!;
					break;
				case "Activity":
					entry = JsonSerializer.Deserialize<Activity>(root.GetRawText(), options)!;
					break;
				default:
					throw new JsonException($"Unknown entry type: {type}");
			}

			return entry;
		}
	}

	public override void Write(Utf8JsonWriter writer, Entry value, JsonSerializerOptions options)
	{
		// Dodajemy pole "Type" aby łatwo rozpoznać klasę przy odczycie
		var type = value is Meal ? "Meal" : value is Activity ? "Activity" : "Entry";

		var json = JsonSerializer.SerializeToElement(value, value.GetType(), options);
		using (writer)
		{
			writer.WriteStartObject();
			writer.WriteString("Type", type);

			foreach (var prop in json.EnumerateObject())
			{
				prop.WriteTo(writer);
			}

			writer.WriteEndObject();
		}
	}
}
