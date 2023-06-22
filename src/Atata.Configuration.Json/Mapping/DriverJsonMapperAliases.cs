namespace Atata.Configuration.Json;

public static class DriverJsonMapperAliases
{
    private static readonly Dictionary<string, IDriverJsonMapper> s_aliasMapperMap = new(StringComparer.OrdinalIgnoreCase);

    static DriverJsonMapperAliases()
    {
        Register<RemoteDriverJsonMapper>(DriverAliases.Remote);
        Register<ChromeDriverJsonMapper>(DriverAliases.Chrome);
        Register<FirefoxDriverJsonMapper>(DriverAliases.Firefox);
        Register<InternetExplorerDriverJsonMapper>(DriverAliases.InternetExplorer);
        Register<SafariDriverJsonMapper>(DriverAliases.Safari);
        Register<EdgeDriverJsonMapper>(DriverAliases.Edge);
    }

    public static void Register<T>(string alias)
        where T : IDriverJsonMapper, new() =>
        Register(alias, new T());

    public static void Register(string alias, IDriverJsonMapper mapper)
    {
        alias.CheckNotNullOrWhitespace(nameof(alias));
        mapper.CheckNotNull(nameof(mapper));

        s_aliasMapperMap[alias.ToLowerInvariant()] = mapper;
    }

    public static IDriverJsonMapper Resolve(string alias) =>
        s_aliasMapperMap.TryGetValue(alias ?? DriverAliases.Remote, out IDriverJsonMapper mapper)
            ? mapper
            : throw new ArgumentException(
                $"There is no JSON mapper defined for \"{alias}\" driver alias. " +
                $"Use one of predefined mappers or {nameof(DriverJsonMapperAliases)}.{nameof(Register)} method to register custom driver JSON mapper.",
                nameof(alias));
}
