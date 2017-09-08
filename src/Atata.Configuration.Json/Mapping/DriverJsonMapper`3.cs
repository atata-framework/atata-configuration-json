using System;
using System.Linq;
using OpenQA.Selenium;

namespace Atata
{
    public abstract class DriverJsonMapper<TBuilder, TService, TOptions> : IDriverJsonMapper
            where TBuilder : DriverAtataContextBuilder<TBuilder, TService, TOptions>
            where TService : DriverService
            where TOptions : DriverOptions, new()
    {
        public void Map(DriverJsonSection section, AtataContextBuilder builder)
        {
            TBuilder driverBuilder = CreateDriverBuilder(builder);

            Map(section, driverBuilder);
        }

        public DriverOptions CreateOptions(DriverOptionsJsonSection section)
        {
            TOptions options = new TOptions();

            MapOptions(section, options);

            return options;
        }

        protected abstract TBuilder CreateDriverBuilder(AtataContextBuilder builder);

        protected virtual void Map(DriverJsonSection section, TBuilder builder)
        {
            if (section.CommandTimeout != null)
                builder.WithCommandTimeout(TimeSpan.FromSeconds(section.CommandTimeout.Value));

            if (!string.IsNullOrWhiteSpace(section.Service?.DriverPath))
                builder.WithDriverPath(section.Service.DriverPath);

            if (!string.IsNullOrWhiteSpace(section.Service?.DriverExecutableFileName))
                builder.WithDriverExecutableFileName(section.Service.DriverExecutableFileName);

            if (section.Options != null)
                builder.WithOptions(opt => MapOptions(section.Options, opt));

            if (section.Service != null)
                builder.WithDriverService(srv => MapService(section.Service, srv));
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
    }
}
