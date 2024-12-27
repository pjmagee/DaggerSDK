namespace Dagger;

public class IdValue<TId>(IId<TId> value) : IFormattedValue where TId : Scalar
{  
    public string Value()
    {
        var id = value.IdAsync().Result;
        return new StringValue(id.Value).Value();
    }
}