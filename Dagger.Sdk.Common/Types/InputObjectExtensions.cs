using System.Reflection;

namespace Dagger;

public static class InputObjectExtensions
{
    public static Dictionary<string, IFormattedValue> AsDictionary(this IInputObject instance)
    {
        return instance
            .GetType()
            .GetProperties(BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public)
            .ToDictionary(keySelector => keySelector.Name, valueSelector => valueSelector.ToFormattedValue(instance));
    }

    static IFormattedValue ToFormattedValue(this PropertyInfo propertyInfo, IInputObject instance)
    {
        if (propertyInfo.PropertyType == typeof(string)) 
            return new StringValue(propertyInfo.GetValue(instance)!.ToString()!);
        
        if (propertyInfo.PropertyType == typeof(int))
            return new IntValue((int)propertyInfo.GetValue(instance)!);
        
        if (propertyInfo.PropertyType == typeof(float))
            return new FloatValue((float)propertyInfo.GetValue(instance)!);
        
        if (propertyInfo.PropertyType == typeof(bool))
            return new BooleanValue((bool)propertyInfo.GetValue(instance)!);
        
        if (propertyInfo.PropertyType == typeof(Enum))
            return new StringValue(Enum.GetName(propertyInfo.PropertyType, propertyInfo.GetValue(instance)!)!);
        
        if (propertyInfo.PropertyType.IsGenericType && propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            var type = Nullable.GetUnderlyingType(propertyInfo.PropertyType);
            var value = propertyInfo.GetValue(instance);
            
            if (type == typeof(int) && value is not null)
                return new IntValue((int)value!);
            
            if (type == typeof(float) && value is not null)
                return new FloatValue((float)value!);
            
            if (type == typeof(bool) && value is not null)
                return new BooleanValue((bool)value!);
            
            if (type == typeof(string) && value is not null)
                return new StringValue((string)value!);
            
            if (type == typeof(Enum) && value is not null)
                return new StringValue(Enum.GetName(type, value!)!);
        }
        
        throw new Exception($"Unsupported type {propertyInfo.PropertyType.Name} in {propertyInfo.Name}");
    }
}