namespace Dagger;

public record Argument(string Key, Value Value)
{
    public Task<string> FormatValue() => Value.FormatAsync();
}