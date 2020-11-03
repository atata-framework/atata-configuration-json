using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Atata.Configuration.Json
{
    public class AttributeMapper
    {
        private static readonly Dictionary<string, string> AlternativeParameterNamesMap = new Dictionary<string, string>
        {
            ["value"] = "values",
            ["case"] = "termCase"
        };

        private readonly Assembly[] assembliesToFindAttributeTypes;

        private readonly IObjectCreator objectCreator;

        public AttributeMapper(string assemblyNamePatternToFindAttributeTypes, string defaultAssemblyNamePatternToFindTypes)
        {
            assembliesToFindAttributeTypes = AssemblyFinder.FindAllByPattern(assemblyNamePatternToFindAttributeTypes);

            IObjectConverter objectConverter = new ObjectConverter
            {
                AssemblyNamePatternToFindTypes = defaultAssemblyNamePatternToFindTypes
            };
            IObjectMapper objectMapper = new ObjectMapper(objectConverter);
            objectCreator = new ObjectCreator(objectConverter, objectMapper);
        }

        public Attribute Map(AttributeJsonSection section)
        {
            if (string.IsNullOrEmpty(section.Type))
                throw new ConfigurationException(
                    "\"type\" configuration property of attribute section is not specified.");

            string typeName = NormalizeAttributeTypeName(section.Type);

            Type attributeType = TypeFinder.FindInAssemblies(typeName, assembliesToFindAttributeTypes);

            if (!typeof(Attribute).IsAssignableFrom(attributeType))
                throw new ConfigurationException(
                    $"\"type\"=\"{section.Type}\" configuration property of attribute secton doesn't reference an attribute type.");

            var valuesMap = section.ExtraPropertiesMap.ToDictionary(
                x => x.Key,
                x => PostProcessConfigurationValue(x.Key, x.Value));

            return (Attribute)objectCreator.Create(attributeType, valuesMap, AlternativeParameterNamesMap);
        }

        private static object PostProcessConfigurationValue(string name, object value)
        {
            if (name.EndsWith("AttributeType"))
                if (value is string stringValue)
                    return NormalizeAttributeTypeName(stringValue);

            if (name.EndsWith("AttributeTypes"))
                if (value is object[] arrayValue)
                    return arrayValue.Select(x => NormalizeAttributeTypeName(x.ToString())).ToArray();

            return value;
        }

        private static string NormalizeAttributeTypeName(string typeName)
        {
            return !typeName.Contains(",") && !typeName.EndsWith(nameof(Attribute), StringComparison.OrdinalIgnoreCase)
                ? typeName + nameof(Attribute)
                : typeName;
        }
    }
}
