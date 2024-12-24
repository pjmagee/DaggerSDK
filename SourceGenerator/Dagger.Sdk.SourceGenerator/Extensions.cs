using System;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Dagger.GraphQL;
using Microsoft.CodeAnalysis.CSharp;

public static class Extensions
{
    public static string ToPascalCase(this string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return input;

        var words = Regex.Split(input, @"(?<!^)(?=[A-Z])|[_\-\s]+");
        TextInfo textInfo = CultureInfo.InvariantCulture.TextInfo;
        return string.Concat(words.Select(word => textInfo.ToTitleCase(word.ToLowerInvariant())));
    }

    public static string GetDefaultValue(this InputValue f)
    {
        if (f.DefaultValue is null) return string.Empty;
        if (string.IsNullOrWhiteSpace(f.DefaultValue.Trim('"'))) return string.Empty;
        bool isReservedKeyWord = SyntaxFacts.GetKeywordKind(f.DefaultValue.Trim('"')) != SyntaxKind.None;
        return isReservedKeyWord ? $"@{f.DefaultValue}" : f.DefaultValue.Trim('"');
    }

    public static string GetArgumentName(this InputValue f)
    {
        bool isReservedKeyword = SyntaxFacts.GetKeywordKind(f.Name) != SyntaxKind.None;
        return isReservedKeyword ? $"@{f.Name}" : f.Name;
    }

    public static string GetMethodName(this Dagger.GraphQL.Type  type, Field f)
    {
        var name = f.Name.ToPascalCase();

        if (f.Type.IsLeaf() || f.Type.IsList())
        {
            return $"{name}Async";
            ;
        }

        if (type.Name.Equals(f.Name, StringComparison.OrdinalIgnoreCase))
        {
            return $"{name}_";
        }

        return name;
    }

    public static string GetTypeName(this TypeRef f)
    {
        return f.Kind switch
        {
            Kind.NON_NULL => GetTypeName(f.OfType),
            Kind.LIST => $"{GetTypeName(f.OfType)}",
            Kind.SCALAR => f.GetScalarTypeName(),
            _ => f.Name
        };
    }

    public static bool IsNullable(this TypeRef f)
    {
        return f.Kind != Kind.NON_NULL;
    }

    public static bool IsFloat(this TypeRef f)
    {
        return f.Kind == Kind.SCALAR && f.GetScalarTypeName() == SyntaxFactory.Token(SyntaxKind.FloatKeyword).ToFullString();
    }

    public static bool IsInt(this TypeRef f)
    {
        return f.Kind == Kind.SCALAR && f.GetScalarTypeName() == SyntaxFactory.Token(SyntaxKind.IntKeyword).ToFullString();
    }

    public static bool IsString(this TypeRef f)
    {
        return f.Kind == Kind.SCALAR && f.GetScalarTypeName() == SyntaxFactory.Token(SyntaxKind.StringKeyword).ToFullString();
    }

    public static bool IsBoolean(this TypeRef f)
    {
        return f.Kind == Kind.SCALAR && f.GetScalarTypeName() == SyntaxFactory.Token(SyntaxKind.BoolKeyword).ToFullString();
    }

    public static string GetScalarTypeName(this TypeRef f)
    {
        return f.Name.ToLower() switch
        {
            "string" => SyntaxFactory.Token(SyntaxKind.StringKeyword).ToFullString(),
            "int" => SyntaxFactory.Token(SyntaxKind.IntKeyword).ToFullString(),
            "float" => SyntaxFactory.Token(SyntaxKind.FloatKeyword).ToFullString(),
            "boolean" => SyntaxFactory.Token(SyntaxKind.BoolKeyword).ToFullString(),
            _ => f.Name
        };
    }

    /// <summary>
    /// Get the optional arguments from a list of arguments.
    /// </summary>
    public static ImmutableArray<InputValue> OptionalArgs(this InputValue[] args) => [..args.Where(arg => arg.Type.Kind != Kind.NON_NULL)];

    /// <summary>
    /// Get the required arguments from a list of arguments.
    /// </summary>
    public static ImmutableArray<InputValue> RequiredArgs(this InputValue[] args) => [..args.Where(arg => arg.Type.Kind == Kind.NON_NULL)];

    public static Dagger.GraphQL.Type[] ExcludeInternalTypes(this Dagger.GraphQL.Type[] types) => types.Where(t => !t.Name.StartsWith('_')).ToArray();

    public static bool IsLeaf(this TypeRef t)
    {
        var tr = t;

        if (t.Kind == Kind.NON_NULL)
        {
            tr = t.OfType;
        }

        if (tr.Kind == Kind.ENUM)
        {
            return true;
        }

        if (tr.Kind == Kind.SCALAR)
        {
            return true;
        }

        return false;
    }

    public static bool IsList(this TypeRef t)
    {
        var tr = t;

        if (t.Kind == Kind.NON_NULL)
        {
            tr = t.OfType;
        }

        if (tr.Kind == Kind.LIST)
        {
            return true;
        }

        return false;
    }

    public static bool IsEnum(this TypeRef t)
    {
        var tr = t;

        if (t.Kind == Kind.NON_NULL)
        {
            tr = t.OfType;
        }

        if (tr.Kind == Kind.ENUM)
        {
            return true;
        }

        return false;
    }

    public static bool IsInputObject(this TypeRef t)
    {
        var tr = t;

        if (t.Kind == Kind.NON_NULL)
        {
            tr = t.OfType;
        }

        if (tr.Kind == Kind.INPUT_OBJECT)
        {
            return true;
        }

        return false;
    }

    public static bool IsScalar(this TypeRef t)
    {
        var tr = t;

        if (t.Kind == Kind.NON_NULL)
        {
            tr = t.OfType;
        }

        if (tr.Kind == Kind.SCALAR)
        {
            return true;
        }

        return false;
    }

    public static bool IsObject(this TypeRef t)
    {
        var tr = t;

        if (t.Kind == Kind.NON_NULL)
        {
            tr = t.OfType;
        }

        if (tr.Kind == Kind.OBJECT)
        {
            return true;
        }

        return false;
    }

    public static TypeRef GetType_(this TypeRef t)
    {
        var tr = t;

        if (t.Kind == Kind.NON_NULL)
        {
            tr = t.OfType;
        }

        return tr;
    }
}