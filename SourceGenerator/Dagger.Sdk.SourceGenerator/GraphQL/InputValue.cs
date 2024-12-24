using System.Text.Json.Serialization;

namespace Dagger.GraphQL;

public class InputValue
{
    [JsonPropertyName("defaultValue")]
    public string? DefaultValue { get; set; }
    
    [JsonPropertyName("deprecationReason")]
    public string? DeprecationReason { get; set; }
    
    [JsonPropertyName("description")]
    public required string Description { get; set; }
    
    [JsonPropertyName("isDeprecated")]
    public required bool IsDeprecated { get; set; }
    
    [JsonPropertyName("directives")]
    public required object[] Directives { get; set; }
    
    [JsonPropertyName("name")]
    public required string Name { get; set; }
    
    [JsonPropertyName("type")]
    public required TypeRef Type { get; set; }
}