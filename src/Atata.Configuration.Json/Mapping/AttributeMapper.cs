namespace Atata.Configuration.Json;

public class AttributeMapper
{
    private static readonly Dictionary<string, string> s_alternativeParameterNamesMap = new()
    {
        ["value"] = "values",
        ["case"] = "termCase"
    };

    private readonly Assembly[] _assembliesToFindAttributeTypes;

    private readonly IObjectCreator _objectCreator;

    public AttributeMapper(string assemblyNamePatternToFindAttributeTypes, string defaultAssemblyNamePatternToFindTypes)
    {
        _assembliesToFindAttributeTypes = AssemblyFinder.FindAllByPattern(assemblyNamePatternToFindAttributeTypes);

        IObjectConverter objectConverter = new ObjectConverter
        {
            AssemblyNamePatternToFindTypes = defaultAssemblyNamePatternToFindTypes
        };
        IObjectMapper objectMapper = new ObjectMapper(objectConverter);
        _objectCreator = new ObjectCreator(objectConverter, objectMapper);
    }

    public Attribute Map(AttributeJsonSection section)
    {
        if (string.IsNullOrEmpty(section.Type))
            throw new ConfigurationException(
                "\"type\" configuration property of attribute section is not specified.");

        string typeName = NormalizeAttributeTypeName(section.Type);

        Type attributeType = TypeFinder.FindInAssemblies(typeName, _assembliesToFindAttributeTypes);

        if (!typeof(Attribute).IsAssignableFrom(attributeType))
            throw new ConfigurationException(
                $"\"type\"=\"{section.Type}\" configuration property of attribute section doesn't reference an attribute type.");

        var valuesMap = section.ExtraPropertiesMap.ToDictionary(
            x => x.Key,
            x => PostProcessConfigurationValue(x.Key, x.Value));

        return (Attribute)_objectCreator.Create(attributeType, valuesMap, s_alternativeParameterNamesMap);
    }

    private static object PostProcessConfigurationValue(string name, object value)
    {
        if (name.EndsWith("AttributeType", StringComparison.Ordinal) && value is string stringValue)
            return NormalizeAttributeTypeName(stringValue);

        if (name.EndsWith("AttributeTypes", StringComparison.Ordinal) && value is object[] arrayValue)
            return arrayValue.Select(x => NormalizeAttributeTypeName(x.ToString())).ToArray();

        return value;
    }

    private static string NormalizeAttributeTypeName(string typeName) =>
        !typeName.Contains(",") && !typeName.EndsWith(nameof(Attribute), StringComparison.OrdinalIgnoreCase)
            ? typeName + nameof(Attribute)
            : typeName;
}
