using System.Text.Json.Serialization;

namespace Dagger.GraphQL;

public class Field
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }
    
    [JsonPropertyName("description")]
    public required string Description { get; set; }
    
    [JsonPropertyName("deprecationReason")]
    public string? DeprecationReason { get; set; }
    
    [JsonPropertyName("isDeprecated")]
    public required bool IsDeprecated { get; set; }
    
    [JsonPropertyName("args")]
    public required InputValue[] Args { get; set; }
    
    [JsonPropertyName("type")]
    public required TypeRef Type { get; set; }
}