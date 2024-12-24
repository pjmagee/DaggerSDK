namespace Dagger;

public class IntValue(int n) : Value
{
    public override Task<string> FormatAsync()
    {
        return Task.FromResult(n.ToString());
    }
}