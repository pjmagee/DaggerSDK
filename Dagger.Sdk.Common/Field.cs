using System.Collections.Immutable;

namespace Dagger;

public class Field(string name, ImmutableList<Argument> args)
{
    public string Name { get; } = name;

    public ImmutableList<Argument> Args { get; } = args;
}