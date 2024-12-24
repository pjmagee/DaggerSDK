using System.Text.Json.Serialization;

namespace Dagger.GraphQL;

public class Schema
{
    [JsonPropertyName("types")]
    public required Type[] Types { get; set; }
}