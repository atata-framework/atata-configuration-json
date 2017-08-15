using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Atata
{
    public class ObjectDictionaryJsonSection : JsonSection
    {
        public Dictionary<string, object> Values => AdditionalProperties?.ToDictionary(x => x.Key, x => ConvertJToken(x.Value));

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
                default:
                    return (string)token;
            }
        }
    }
}
