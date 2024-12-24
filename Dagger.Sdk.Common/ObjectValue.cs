using System.Text;

namespace Dagger;

public class ObjectValue(List<KeyValuePair<string, Value>> obj) : Value
{
    public override Task<string> FormatAsync()
    {
        var builder = new StringBuilder();
        builder.Append('{');
        builder.Append(
            string.Join(
                ",",
                obj.Select(async kv => $"{kv.Key}:{await kv.Value.FormatAsync()}").Select(v => v.Result)
            )
        );
        builder.Append('}');
        return Task.FromResult(builder.ToString());
    }
}