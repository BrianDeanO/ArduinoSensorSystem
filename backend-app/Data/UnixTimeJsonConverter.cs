using Newtonsoft.Json;

/// <summary>
/// It's easier for devices to provide a Unix Epoch timestamp rather than a formatted
/// time string when submitting data points, this allows us to seamlessly read either
/// formatted time strings or Unix Epoch timestamps, in the form of json integer values.
/// </summary>
public class UnixTimeJsonConverter : JsonConverter
{
	public override bool CanConvert(Type objectType)
	{
		return objectType == typeof(DateTime);
	}

	public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
	{
		if(reader.TokenType == JsonToken.Integer) {
			long millisecondsSinceEpoch = serializer.Deserialize<long>(reader);
			return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(millisecondsSinceEpoch);
		}
		else if (reader.TokenType == JsonToken.String) {
			// Parse string representation
			string? dateTimeString = serializer.Deserialize<string>(reader);
			if(dateTimeString is not null) {
				return DateTime.Parse(dateTimeString);
			}
		}
		throw new JsonSerializationException("Unexpected token type when parsing DateTime.");
	}

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
		if(value is not null) {
			DateTime dateTime = (DateTime)value;
			writer.WriteValue(dateTime.ToString("yyyy-MM-ddTHH:mm:ssZ")); // ISO 8601 format
		}
		else {
			writer.WriteNull();
		}
    }
}