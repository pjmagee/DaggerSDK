using System.Collections.Immutable;
using System.Diagnostics;
using System.Reflection;

namespace Dagger;

/// <summary>
/// Should be part of code gen, but for now is a utility to build arguments for a query.
/// </summary>
public static class ArgumentsBuilder
{
    public static ImmutableList<Argument> GetArguments(params object?[] args)
    {
        var method = new StackFrame(1).GetMethod()!;
        var parameters = method.GetParameters();
        List<Argument> arguments = [];

        for (int i = 0; i < parameters.Length; i++)
        {
            ParameterInfo pi = parameters[i];
            object? value = args[i];
            
            // If the value is actually NULL, do we even need to build it?
            if (value == null) continue;
            
            if (pi.ParameterType.IsArray)
            {
                Type elementType = pi.ParameterType.GetElementType()!;
                
                if (elementType == typeof(string))
                {
                    arguments.Add(new Argument(pi.Name!, Strings(value)));
                }
                else if (elementType == typeof(bool))
                {
                    arguments.Add(new Argument(pi.Name!, Bools(value)));
                }
                else if (elementType == typeof(int))
                {
                    arguments.Add(new Argument(pi.Name!, Ints(value)));
                }
                else if (elementType == typeof(float))
                {
                    arguments.Add(new Argument(pi.Name!, Floats(value)));
                }
                else if (elementType.IsEnum)
                {
                    arguments.Add(new Argument(pi.Name!, Enums(value)));
                }
                else if (elementType.IsAssignableTo(typeof(IInputObject)))
                {
                    arguments.Add(new Argument(pi.Name!, InputObjects(value)));
                }
                else if (elementType == typeof(Scalar))
                {
                    arguments.Add(new Argument(pi.Name!, Scalars(value)));
                }
                else if(elementType.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IId<>)))
                {
                    arguments.Add(new Argument(pi.Name!, Ids(value, pi)));
                }
            }
            else if(Nullable.GetUnderlyingType(pi.ParameterType) != null)
            {
                Type type = Nullable.GetUnderlyingType(pi.ParameterType)!;
                
                if (type == typeof(string))
                {
                    arguments.Add(new Argument(pi.Name!, new StringValue((string)value!)));
                }
                else if (type == typeof(int))
                {
                    arguments.Add(new Argument(pi.Name!, new IntValue((int)value!)));
                }
                else if (type == typeof(float))
                {
                    arguments.Add(new Argument(pi.Name!, new FloatValue((float)value!)));
                }
                else if (type == typeof(bool))
                {
                    arguments.Add(new Argument(pi.Name!, new BooleanValue((bool)value!)));
                }
                else if (type.IsEnum)
                {
                    arguments.Add(new Argument(pi.Name!, new StringValue(Enum.GetName(type, value!)!)));
                }
                else if (type == typeof(Scalar))
                {
                    arguments.Add(new Argument(pi.Name!, new StringValue(((Scalar)value).Value)));
                }
                else if(type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IId<>)))
                {
                    arguments.Add(new Argument(pi.Name!, IId(pi, value)));
                }
            }
            else
            {
                if (pi.ParameterType == typeof(string))
                {
                    arguments.Add(new Argument(pi.Name!, new StringValue((string)value!)));
                }
                else if (pi.ParameterType == typeof(int))
                {
                    arguments.Add(new Argument(pi.Name!, new IntValue((int)value!)));
                }
                else if (pi.ParameterType == typeof(float))
                {
                    arguments.Add(new Argument(pi.Name!, new FloatValue((float)value!)));
                }
                else if (pi.ParameterType == typeof(bool))
                {
                    arguments.Add(new Argument(pi.Name!, new BooleanValue((bool)value!)));
                }
                else if (pi.ParameterType.IsEnum)
                {
                    arguments.Add(new Argument(pi.Name!, new StringValue(Enum.GetName(pi.ParameterType, value!)!)));
                }
                else if (pi.ParameterType == typeof(Scalar))
                {
                    arguments.Add(new Argument(pi.Name!, new StringValue(((Scalar)value).Value)));
                }
                else if(pi.ParameterType.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IId<>)))
                {
                    arguments.Add(new Argument(pi.Name!, IId(pi, value)));
                }
            }
        }
            
        return arguments.ToImmutableList();
    }

    static IFormattedValue Enums(object value)
    {
        return new ListValue(((Enum[])value).Select(e => new StringValue(e.ToString())).Cast<IFormattedValue>().ToArray());
    }

    static IFormattedValue IId(ParameterInfo pi, object value)
    {
        var idValue = typeof(IdValue<>);
        var genericTypeArgument = Type.GetType(pi.ParameterType.Name + "ID, Dagger.Sdk")!;
        var constructed = idValue!.MakeGenericType(genericTypeArgument);
        var formattedValue = (IFormattedValue) Activator.CreateInstance(constructed, value)!;
        return formattedValue;
    }

    static IFormattedValue Scalars(object value)
    {
        return new ListValue(((Scalar[])value).Select(s => new StringValue(s.Value)).Cast<IFormattedValue>().ToArray());
    }

    static IFormattedValue Ids(object value, ParameterInfo pi)
    {
        var idValue = typeof(IdValue<>);
        var genericTypeArgument = Type.GetType(pi.ParameterType.Name + "ID, Dagger.Sdk")!;
        var constructed = idValue!.MakeGenericType(genericTypeArgument);
        var items = ((object[])value).Select(v => Activator.CreateInstance(constructed, v)).Cast<IFormattedValue>().ToArray();
        return new ListValue(items);
    }

    static IFormattedValue InputObjects(object value)
    {
        var items = ConvertStructArrayToObjectArray(value);
        return new ListValue(items.Select(v => new ObjectValue(((IInputObject)v).ToDictionary())).Cast<IFormattedValue>().ToArray());
    }

    static IFormattedValue Floats(object value)
    {
        return new ListValue(((float[])value).Select(f => new FloatValue(f)).Cast<IFormattedValue>().ToArray());
    }

    static IFormattedValue Ints(object value)
    {
        return new ListValue(((int[])value).Select(n => new IntValue(n)).Cast<IFormattedValue>().ToArray());
    }

    static IFormattedValue Bools(object value)
    {
        return new ListValue(((bool[])value).Select(b => new BooleanValue(b)).Cast<IFormattedValue>().ToArray());
    }

    static IFormattedValue Strings(object value)
    {
        return new ListValue((value as string[])!.Select(s => new StringValue(s)).Cast<IFormattedValue>().ToArray());
    }

    static object[] ConvertStructArrayToObjectArray(object value)
    {
        if (value is Array array)
        {
            return array.Cast<object>().ToArray();
        }

        throw new InvalidCastException("The provided value is not a valid array.");
    }
}