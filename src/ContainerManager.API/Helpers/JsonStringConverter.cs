using System;
using System.Text.Json;

namespace ContainerManager.API.Helpers
{
	public class JsonStringConverter : System.Text.Json.Serialization.JsonConverter<string>
	{
		public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if (reader.TokenType == JsonTokenType.Number)
			{
				var stringValue = reader.GetInt32();
				return stringValue.ToString();
			}
			else if (reader.TokenType == JsonTokenType.String)
			{
				return reader.GetString();
			}

			throw new JsonException();
		}

		public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options) => writer.WriteStringValue(value);
	}
}
