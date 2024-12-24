namespace Dagger;

public interface IInputObject
{
    List<KeyValuePair<string, Value>> ToKeyValuePairs();
}