using System.Text;

namespace Dagger;

public class ListValue(List<Value> list) : Value
{
    public override Task<string> FormatAsync()
    {
        var builder = new StringBuilder();
        builder.Append('[');
        builder.Append(
            string.Join(
                ",",
                list.Select(async element => await element.FormatAsync()).Select(v => v.Result)
            )
        );
        builder.Append(']');
        return Task.FromResult(builder.ToString());
    }
}