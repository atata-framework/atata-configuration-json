using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Atata
{
    public class JsonSection
    {
        [JsonExtensionData]
        public Dictionary<string, JToken> AdditionalProperties { get; } = new Dictionary<string, JToken>();

        public T Get<T>(string propertyName)
        {
            string normalizedPropertyName = propertyName.ToString(TermCase.Camel);

            JToken token;

            return AdditionalProperties.TryGetValue(normalizedPropertyName, out token)
                || AdditionalProperties.TryGetValue(propertyName, out token)
                ? token.ToObject<T>()
                : default(T);
        }
    }
}
