namespace Dagger;

public interface IInputObject
{
    public Dictionary<string, IFormattedValue> ToDictionary();
}