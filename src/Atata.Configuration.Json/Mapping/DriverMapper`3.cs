using System;
using System.Linq;
using OpenQA.Selenium;

namespace Atata
{
    public abstract class DriverMapper<TBuilder, TService, TOptions>
            where TBuilder : DriverAtataContextBuilder<TBuilder, TService, TOptions>
            where TService : DriverService
            where TOptions : DriverOptions, new()
    {
        protected abstract TBuilder CreateDriverBuilder(AtataContextBuilder builder);

        public void Map(DriverJsonSection config, AtataContextBuilder builder)
        {
            TBuilder driverBuilder = CreateDriverBuilder(builder);

            Map(config, driverBuilder);
        }

        protected virtual void Map(DriverJsonSection config, TBuilder builder)
        {
            if (config.CommandTimeout != null)
                builder.WithCommandTimeout(TimeSpan.FromSeconds(config.CommandTimeout.Value));

            if (!string.IsNullOrWhiteSpace(config.Service?.DriverPath))
                builder.WithDriverPath(config.Service.DriverPath);

            if (!string.IsNullOrWhiteSpace(config.Service?.DriverExecutableFileName))
                builder.WithDriverExecutableFileName(config.Service.DriverExecutableFileName);

            if (config.Options != null)
                builder.WithOptions(opt => MapOptions(config.Options, opt));

            if (config.Service != null)
                builder.WithDriverService(srv => MapService(config.Service, srv));
        }

        protected virtual void MapOptions(DriverOptionsJsonSection section, TOptions options)
        {
            var properties = section.ExtraPropertiesMap;

            if (properties?.Any() ?? false)
                AtataMapper.Map(properties, options);

            if (section.LoggingPreferences?.Any() ?? false)
            {
                foreach (var item in section.LoggingPreferences)
                    options.SetLoggingPreference(item.Key, item.Value);
            }

            if (section.AdditionalCapabilities != null)
            {
                foreach (var item in section.AdditionalCapabilities.ExtraPropertiesMap)
                    options.AddAdditionalCapability(item.Key, item.Value);
            }
        }

        protected virtual void MapService(DriverServiceJsonSection section, TService service)
        {
            var properties = section.ExtraPropertiesMap;

            if (properties?.Any() ?? false)
                AtataMapper.Map(properties, service);
        }

        protected Proxy CreateProxy(ProxyJsonSection section)
        {
            Proxy proxy = new Proxy();

            if (section.Kind != null)
                proxy.Kind = section.Kind.Value;

            if (!string.IsNullOrWhiteSpace(section.HttpProxy))
                proxy.HttpProxy = section.HttpProxy;

            // TODO: Continue...

            return proxy;
        }
    }
}
