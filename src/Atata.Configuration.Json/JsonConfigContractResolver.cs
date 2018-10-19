using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Atata.Configuration.Json
{
    internal class JsonConfigContractResolver : DefaultContractResolver
    {
        public static JsonConfigContractResolver Instance { get; } = new JsonConfigContractResolver();

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            if (property.DeclaringType.IsGenericType && property.DeclaringType.GetGenericTypeDefinition() == typeof(JsonConfig<>) && property.PropertyName == nameof(JsonConfig.Driver))
                property.ShouldSerialize = _ => false;

            return property;
        }
    }
}
