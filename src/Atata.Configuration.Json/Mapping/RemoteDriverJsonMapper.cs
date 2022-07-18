using System;
using OpenQA.Selenium;

namespace Atata.Configuration.Json
{
    public class RemoteDriverJsonMapper : IDriverJsonMapper
    {
        public void Map(DriverJsonSection section, AtataContextBuilder builder)
        {
            RemoteDriverAtataContextBuilder driverBuilder = CreateDriverBuilder(builder);

            Map(section, driverBuilder);
        }

        public DriverOptions CreateOptions(DriverOptionsJsonSection section)
        {
            IDriverJsonMapper mapper = GetOptionsMapper(section.Type);
            return mapper.CreateOptions(section);
        }

        protected virtual RemoteDriverAtataContextBuilder CreateDriverBuilder(AtataContextBuilder builder)
        {
            return builder.UseRemoteDriver();
        }

        protected virtual void Map(DriverJsonSection section, RemoteDriverAtataContextBuilder builder)
        {
            if (!string.IsNullOrWhiteSpace(section.Alias))
                builder.WithAlias(section.Alias);

            if (section.CommandTimeout != null)
                builder.WithCommandTimeout(TimeSpan.FromSeconds(section.CommandTimeout.Value));

            if (!string.IsNullOrWhiteSpace(section.RemoteAddress))
                builder.WithRemoteAddress(section.RemoteAddress);

            if (section.Options != null)
                builder.WithOptions(() => CreateOptions(section.Options));
        }

        private static IDriverJsonMapper GetOptionsMapper(string typeName)
        {
            switch (typeName?.ToLowerInvariant())
            {
                case DriverAliases.Chrome:
                    return new ChromeDriverJsonMapper();
                case DriverAliases.Firefox:
                    return new FirefoxDriverJsonMapper();
                case DriverAliases.InternetExplorer:
                    return new InternetExplorerDriverJsonMapper();
                case DriverAliases.Safari:
                    return new SafariDriverJsonMapper();
                case DriverAliases.Edge:
                    return new EdgeDriverJsonMapper();
                case null:
                    throw new ArgumentNullException(nameof(typeName), "Options type name is not defined.");
                default:
                    throw new ArgumentException($"Unsupported options type name: {typeName}.", nameof(typeName));
            }
        }
    }
}
