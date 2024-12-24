using System.Text.Json;
using System.Text.Json.Serialization;

namespace Dagger;

public class ScalarIdConverter<TScalar> : JsonConverter<TScalar> where TScalar : Scalar, new()
{
    public override TScalar Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var scalar = new TScalar();
        scalar.Value = reader.GetString()!;
        return scalar;
    }

    public override void Write(Utf8JsonWriter writer, TScalar scalar, JsonSerializerOptions options) => writer.WriteStringValue(scalar.Value);
}