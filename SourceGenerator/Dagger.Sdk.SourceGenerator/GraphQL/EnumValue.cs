using System.Text.Json.Serialization;

namespace Dagger.GraphQL;

public class EnumValue
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("description")]
    public required string Description { get; set; }

    [JsonPropertyName("isDeprecated")]
    public required bool IsDeprecated { get; set; }
    
    [JsonPropertyName("deprecationReason")]
    public string? DeprecationReason { get; set; }
}