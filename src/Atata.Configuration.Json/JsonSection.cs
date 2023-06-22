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

        private static object ConvertJToken(JToken token) =>
            token.Type switch
            {
                JTokenType.None or JTokenType.Null or JTokenType.Undefined =>
                    null,
                JTokenType.Integer =>
                    (int)token,
                JTokenType.Float =>
                    (double)token,
                JTokenType.Boolean =>
                    (bool)token,
                JTokenType.TimeSpan =>
                    (TimeSpan)token,
                JTokenType.Array =>
                    ((JArray)token).Select(ConvertJToken).ToArray(),
                JTokenType.Object =>
                    ((IEnumerable<KeyValuePair<string, JToken>>)token).ToDictionary(x => x.Key, x => ConvertJToken(x.Value)),
                _ => (string)token,
            };
    }
}
