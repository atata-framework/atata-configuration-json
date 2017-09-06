using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Atata
{
    public class JsonSection
    {
        [JsonExtensionData]
        public Dictionary<string, JToken> AdditionalProperties { get; } = new Dictionary<string, JToken>();

        public Dictionary<string, object> ExtraPropertiesMap => AdditionalProperties?.ToDictionary(x => x.Key, x => ConvertJToken(x.Value));

        private static object ConvertJToken(JToken token)
        {
            switch (token.Type)
            {
                case JTokenType.None:
                case JTokenType.Null:
                case JTokenType.Undefined:
                    return null;
                case JTokenType.Integer:
                    return (int)token;
                case JTokenType.Float:
                    return (double)token;
                case JTokenType.Boolean:
                    return (bool)token;
                case JTokenType.TimeSpan:
                    return (TimeSpan)token;
                default:
                    return (string)token;
            }
        }

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
