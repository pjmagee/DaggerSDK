using System.Text.Json.Serialization;

namespace Dagger.GraphQL;

public class TypeRef
{    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [JsonPropertyName("kind")]
    public required Kind Kind { get; set; }
    
    [JsonPropertyName("name")]
    public required string Name { get; set; }
    
    [JsonPropertyName("ofType")]
    public TypeRef? OfType { get; set; }
}