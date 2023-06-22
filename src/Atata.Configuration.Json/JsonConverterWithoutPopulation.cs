using System;
using Newtonsoft.Json;

namespace Atata.Configuration.Json;

public class JsonConverterWithoutPopulation : JsonConverter
{
    public override bool CanConvert(Type objectType) =>
        true;

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) =>
        serializer.Deserialize(reader, objectType);

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) =>
        serializer.Serialize(writer, value);
}
