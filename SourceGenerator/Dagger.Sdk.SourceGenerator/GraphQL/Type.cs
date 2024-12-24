using System.Text.Json.Serialization;

namespace Dagger.GraphQL;

public class Type
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [JsonPropertyName("kind")]
    public required Kind Kind { get; set; }
    
    [JsonPropertyName("name")]
    public required string Name { get; set; }
    
    [JsonPropertyName("fields")]
    public required Field[] Fields { get; set; }
    
    [JsonPropertyName("inputFields")]
    public required InputValue[] InputFields { get; set; }
    
    [JsonPropertyName("description")]
    public required string Description { get; set; }
    
    [JsonPropertyName("enumValues")]
    public required EnumValue[] EnumValues { get; set; }
    
    [JsonPropertyName("interfaces")]
    public required object[] Interfaces { get; set; }
    
    [JsonPropertyName("possibleTypes")]
    public required object[] PossibleTypes { get; set; }
}