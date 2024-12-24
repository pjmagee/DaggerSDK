namespace Dagger;

public class BooleanValue(bool b) : Value
{
    public override Task<string> FormatAsync()
    {
        return Task.FromResult(b ? "true" : "false");
    }
}