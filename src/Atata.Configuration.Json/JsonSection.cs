using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Atata.Configuration.Json
{
    /// <summary>
    /// Represents JSON section.
    /// </summary>
    public class JsonSection
    {
        [JsonExtensionData]
        public Dictionary<string, JToken> AdditionalProperties { get; } = new Dictionary<string, JToken>();

        [JsonIgnore]
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
                case JTokenType.Array:
                    return ((JArray)token).Select(ConvertJToken).ToArray();
                case JTokenType.Object:
                    return ((IEnumerable<KeyValuePair<string, JToken>>)token).ToDictionary(x => x.Key, x => ConvertJToken(x.Value));
                default:
                    return (string)token;
            }
        }
    }
}
