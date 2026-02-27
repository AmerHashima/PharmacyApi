using System.Text.Json;
using System.Text.Json.Serialization;

namespace Pharmacy.Api.Converters;

/// <summary>
/// JSON converter that converts empty strings to null for nullable Guid properties
/// Handles common API issue where frontend sends "" instead of null
/// </summary>
public class NullableGuidConverter : JsonConverter<Guid?>
{
    public override Guid? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
            return null;

        if (reader.TokenType == JsonTokenType.String)
        {
            var stringValue = reader.GetString();
            
            // Convert empty string to null
            if (string.IsNullOrWhiteSpace(stringValue))
                return null;

            // Try parse valid GUID
            if (Guid.TryParse(stringValue, out var guid))
                return guid;

            // Invalid GUID format
            throw new JsonException($"Unable to convert '{stringValue}' to Guid.");
        }

        throw new JsonException($"Unexpected token type {reader.TokenType} when parsing Guid.");
    }

    public override void Write(Utf8JsonWriter writer, Guid? value, JsonSerializerOptions options)
    {
        if (value.HasValue)
            writer.WriteStringValue(value.Value.ToString());
        else
            writer.WriteNullValue();
    }
}
