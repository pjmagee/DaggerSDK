using System.Collections.Immutable;
using System.Text;

namespace Dagger;

public sealed record Argument(string Key, IFormattedValue FormattedValue)
{
    public string Value() => FormattedValue.Value();
}

public class Field(string name, ImmutableList <Argument> args)
{
    public string Name { get; } = name;

    public ImmutableList<Argument> Args { get; } = args;
}

public class QueryBuilder(ImmutableList<Field> path)
{
    public readonly ImmutableList<Field> Path = path;

    private QueryBuilder() : this(ImmutableList<Field>.Empty) { }

    /// <summary>
    /// Select a field with name.
    /// </summary>
    /// <param name="name">The field name.</param>
    /// <returns>A new QueryBuilder instance.</returns>
    public QueryBuilder Select(string name)
    {
        return Select(name, ImmutableList<Argument>.Empty);
    }
    
    /// <summary>
    /// Select a field with name plus arguments.
    /// </summary>
    /// <param name="name">The field name.</param>
    /// <param name="args">The field arguments.</param>
    /// <returns>A new QueryBuilder instance.</returns>
    public QueryBuilder Select(string name, ImmutableList <Argument> args)
    {
        return Select(new Field(name, args));
    }

    public QueryBuilder Select(Field field)
    {
        return new QueryBuilder(Path.Add(field));
    }

    /// <summary>
    /// Build GraphQL query.
    /// </summary>
    /// <returns>GraphQL query string</returns>
    public string Build()
    {
        var builder = new StringBuilder();
        builder.Append("query");
        foreach (var selection in Path)
        {
            builder.Append('{');
            builder.Append(selection.Name);
            if (selection.Args.Count > 0)
            {
                builder.Append('(');
                builder.Append(string.Join(',', selection.Args.Select(arg => $"{arg.Key}:{arg.Value()}")));
                builder.Append(')');
            }
        }
        builder.Append(new string('}', Path.Count));
        return builder.ToString();
    }

    /// <summary>
    /// Create a query builder.
    /// </summary>
    /// <returns>A QueryBuilder instance.</returns>
    public static QueryBuilder Create()
    {
        return new QueryBuilder();
    }
}