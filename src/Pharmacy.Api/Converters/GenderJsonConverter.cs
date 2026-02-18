using System.Text.Json;
using System.Text.Json.Serialization;

namespace Pharmacy.Api.Converters;

public class GenderJsonConverter : JsonConverter<char?>
{
    public override char? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        if (reader.TokenType == JsonTokenType.String)
        {
            var stringValue = reader.GetString();
            
            if (string.IsNullOrEmpty(stringValue))
            {
                return null;
            }

            if (stringValue.Length == 1)
            {
                var gender = char.ToUpperInvariant(stringValue[0]);
                if (gender == 'M' || gender == 'F')
                {
                    return gender;
                }
            }
            
            // Handle common string representations
            var lowerValue = stringValue.ToLowerInvariant();
            return lowerValue switch
            {
                "male" or "m" => 'M',
                "female" or "f" => 'F',
                _ => throw new JsonException($"Invalid gender value: {stringValue}. Expected 'M', 'F', 'Male', or 'Female'.")
            };
        }

        throw new JsonException($"Unexpected token type {reader.TokenType} when parsing gender.");
    }

    public override void Write(Utf8JsonWriter writer, char? value, JsonSerializerOptions options)
    {
        if (value.HasValue)
        {
            writer.WriteStringValue(value.Value.ToString());
        }
        else
        {
            writer.WriteNullValue();
        }
    }
}