using System.Globalization;
using System.Text;
using System.Text.Json;

namespace Dagger;

public interface IFormattedValue
{
    public string Value();
}

public class StringValue(string value) : IFormattedValue
{
    public string Value() => $"\"{JsonEncodedText.Encode(value)}\"";
}

public class BooleanValue(bool b) : IFormattedValue
{
    public string Value() => b ? "true" : "false";
}

public class IntValue(int n) : IFormattedValue
{
    public string Value() => n.ToString();
}

public class FloatValue(float f) : IFormattedValue
{
    public string Value() => f.ToString(CultureInfo.InvariantCulture);
}

public class ObjectValue(IEnumerable<KeyValuePair<string, IFormattedValue>> values) : IFormattedValue
{
    public string Value()
    {
        var builder = new StringBuilder();
        builder.Append('{');
        builder.Append(string.Join(',', values.Select(kv => $"{kv.Key.ToLower()}:{kv.Value.Value()}")));
        builder.Append('}');
        return builder.ToString();
    }
}

public class ListValue(IFormattedValue[] values) : IFormattedValue
{
    public string Value()
    {
        var builder = new StringBuilder();
        builder.Append('[');
        builder.Append(string.Join(',', values.Select(value => value.Value()).ToArray()));
        builder.Append(']');
        return builder.ToString();
    }
}