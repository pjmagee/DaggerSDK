namespace Dagger;

public abstract class Value
{
    public abstract Task<string> FormatAsync();
}