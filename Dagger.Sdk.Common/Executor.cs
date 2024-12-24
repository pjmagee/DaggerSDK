using System.Collections.Immutable;
using System.Text.Json;

namespace Dagger;

public static class Executor
{
    public static async Task<T> Execute<T>(GraphQLClient client, QueryBuilder queryBuilder)
    {
        var jsonElement = await Request(client, queryBuilder);
        jsonElement = TakeJsonElementUntilLast<T>(jsonElement, queryBuilder.Path);
        return jsonElement.GetProperty(queryBuilder.Path.Last().Name).Deserialize<T>()!;
    }

    public static async Task<T[]> ExecuteList<T>(GraphQLClient client, QueryBuilder queryBuilder)
    {
        var jsonElement = await Request(client, queryBuilder);
        jsonElement = TakeJsonElementUntilLast<T>(jsonElement, queryBuilder.Path);
        return jsonElement
            .EnumerateArray()
            .Select(elem => elem.GetProperty(queryBuilder.Path.Last().Name))
            .Select(elem => elem.Deserialize<T>()!)
            .ToArray();
    }

    private static async Task<JsonElement> Request(GraphQLClient client, QueryBuilder queryBuilder)
    {
        var query = queryBuilder.Build();
        var response = await client.RequestAsync(query);
        // TODO: handle error here.
        var data = await response.Content.ReadAsStringAsync();
        var jsonElement = JsonSerializer.Deserialize<JsonElement>(data);
        return jsonElement.GetProperty("data");
    }

    // Traverse jsonElement until the last element.
    private static JsonElement TakeJsonElementUntilLast<T>(JsonElement jsonElement, ImmutableList<Field> path)
    {
        var json = jsonElement;
        foreach (var fieldName in path.RemoveAt(path.Count - 1).Select(field => field.Name))
        {
            json = json.GetProperty(fieldName);
        }

        return json;
    }
}