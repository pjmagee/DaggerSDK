using System.Globalization;

namespace Dagger;

public class FloatValue(float f) : Value
{
    public override Task<string> FormatAsync()
    {
        return Task.FromResult(f.ToString(CultureInfo.CurrentCulture));
    }
}