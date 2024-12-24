using System.Text.Json.Serialization;

namespace Dagger.GraphQL;

/// <summary>
/// This class is used to deserialize the introspection schema from a GraphQL server.
/// </summary>
public class SchemaFile
{
    [JsonPropertyName("__schema")]
    public required Schema Schema { get; set; }
}