using System.Text.Json;

namespace Dagger;

public class StringValue(string value) : Value
{
    public override Task<string> FormatAsync()
    {
        var escapedValue = JsonEncodedText.Encode(value).ToString();
        return Task.FromResult($"\"{escapedValue}\"");
    }
}