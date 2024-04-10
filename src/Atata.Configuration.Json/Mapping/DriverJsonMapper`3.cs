using OpenQA.Selenium;

namespace Atata.Configuration.Json;

public abstract class DriverJsonMapper<TBuilder, TService, TOptions> : IDriverJsonMapper
    where TBuilder : DriverAtataContextBuilder<TBuilder, TService, TOptions>
    where TService : DriverService
    where TOptions : DriverOptions, new()
{
    public const string BaseDirectoryVariable = "{basedir}";

    private readonly Lazy<IObjectMapper> _lazyObjectMapper = new(
        () => new ObjectMapper(new ObjectConverter()));

    protected IObjectMapper ObjectMapper => _lazyObjectMapper.Value;

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
        if (!string.IsNullOrWhiteSpace(section.Alias))
            builder.WithAlias(section.Alias);

        if (section.CreateRetries != null)
            builder.WithCreateRetries(section.CreateRetries.Value);

        if (section.InitialHealthCheck != null)
            builder.WithInitialHealthCheck(section.InitialHealthCheck.Value);

        if (section.CommandTimeout != null)
            builder.WithCommandTimeout(TimeSpan.FromSeconds(section.CommandTimeout.Value));

        if (section.PortsToIgnore is { Length: > 0 })
            builder.WithPortsToIgnore(section.PortsToIgnore);

        if (!string.IsNullOrWhiteSpace(section.Service?.DriverPath))
            builder.WithDriverPath(FormatDriverPath(section.Service.DriverPath));

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

        if (properties is { Count: > 0 })
            ObjectMapper.Map(properties, options);

        if (section.Proxy != null)
        {
            options.Proxy = new Proxy();
            MapProxy(section.Proxy, options.Proxy);
        }

        if (section.AdditionalOptions != null)
        {
            foreach (var item in section.AdditionalOptions.ExtraPropertiesMap)
                options.AddAdditionalOption(item.Key, FillTemplateVariables(item.Value));
        }

        if (section.LoggingPreferences is { Count: > 0 })
        {
            foreach (var item in section.LoggingPreferences)
                options.SetLoggingPreference(item.Key, item.Value);
        }
    }

    private void MapProxy(ProxyJsonSection section, Proxy proxy)
    {
        ObjectMapper.Map(section.ExtraPropertiesMap, proxy);

        if (section.BypassAddresses is { Length: > 0 })
            proxy.AddBypassAddresses(section.BypassAddresses);
    }

    protected virtual void MapService(DriverServiceJsonSection section, TService service)
    {
        var properties = section.ExtraPropertiesMap;

        if (properties is { Count: > 0 })
            ObjectMapper.Map(properties, service);
    }

    private static string FormatDriverPath(string driverPath) =>
        driverPath.Contains(BaseDirectoryVariable)
            ? driverPath.Replace(BaseDirectoryVariable, AppDomain.CurrentDomain.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar))
            : driverPath;

    protected static object FillTemplateVariables(object value) =>
        value is string valueAsString
            ? FillTemplateVariables(valueAsString)
            : value;

    protected static string FillTemplateVariables(string value) =>
        AtataContext.Current?.FillTemplateString(value) ?? value;
}
