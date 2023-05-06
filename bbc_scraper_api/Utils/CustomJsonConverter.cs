using System.Text.Json;
using System.Text.Json.Serialization;

namespace bbc_scraper_api.Utils;

public class CustomJsonConverter : JsonConverter<string>
{
    public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Number)
        {
            double doubleValue = reader.GetDouble();
            int intValue = Convert.ToInt32(doubleValue);
            return intValue.ToString();
        }
        return reader.GetString();
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }

    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(string);
    }
}
