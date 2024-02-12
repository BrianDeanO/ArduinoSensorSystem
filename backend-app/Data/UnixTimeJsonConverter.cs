using System.Text.Json;
using System.Text.Json.Serialization;

/// <summary>
/// It's easier for devices to provide a Unix Epoch timestamp rather than a formatted
/// time string when submitting data points, this allows us to seamlessly read either
/// formatted time strings or Unix Epoch timestamps, in the form of json integer values.
/// </summary>
public class UnixTimeJsonConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Number)
        {
            long millisecondsSinceEpoch = reader.GetInt64();
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(millisecondsSinceEpoch);
        }
        else if (reader.TokenType == JsonTokenType.String)
        {
            // Parse string representation
            string? dateTimeString = reader.GetString();
            if (dateTimeString != null)
            {
                return DateTime.Parse(dateTimeString);
            }
        }
        throw new JsonException("Unexpected token type when parsing DateTime.");
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("yyyy-MM-ddTHH:mm:ssZ")); // ISO 8601 format
    }
}