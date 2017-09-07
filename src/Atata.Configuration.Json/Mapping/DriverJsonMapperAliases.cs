using System;
using System.Collections.Generic;

namespace Atata
{
    public static class DriverJsonMapperAliases
    {
        private static readonly Dictionary<string, IDriverJsonMapper> AliasMapperMap = new Dictionary<string, IDriverJsonMapper>(StringComparer.OrdinalIgnoreCase);

        static DriverJsonMapperAliases()
        {
            Register<ChromeDriverJsonMapper>(Chrome);
            Register<FirefoxDriverJsonMapper>(Firefox);
            Register<InternetExplorerDriverJsonMapper>(InternetExplorer);
            Register<SafariDriverJsonMapper>(Safari);
            Register<OperaDriverJsonMapper>(Opera);
            Register<EdgeDriverJsonMapper>(Edge);
        }

        public static string Chrome => nameof(Chrome);

        public static string Firefox => nameof(Firefox);

        public static string InternetExplorer => nameof(InternetExplorer);

        public static string Safari => nameof(Safari);

        public static string Opera => nameof(Opera);

        public static string Edge => nameof(Edge);

        public static void Register<T>(string alias)
            where T : IDriverJsonMapper, new()
        {
            Register(alias, new T());
        }

        public static void Register(string alias, IDriverJsonMapper mapper)
        {
            alias.CheckNotNullOrWhitespace(nameof(alias));
            mapper.CheckNotNull(nameof(mapper));

            AliasMapperMap[alias.ToLower()] = mapper;
        }

        public static IDriverJsonMapper Resolve(string alias)
        {
            alias.CheckNotNullOrWhitespace(nameof(alias));

            return AliasMapperMap.TryGetValue(alias, out IDriverJsonMapper mapper)
                ? mapper
                : throw new ArgumentException($"There is no JSON mapper defined for \"{alias}\" driver alias. Use {nameof(DriverJsonMapperAliases)}.{nameof(Register)} method to register custom driver JSON mapper.", nameof(alias));
        }
    }
}
