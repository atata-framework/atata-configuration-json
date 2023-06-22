using System.Linq;
using OpenQA.Selenium.Chromium;

namespace Atata.Configuration.Json;

public abstract class ChromiumDriverJsonMapper<TBuilder, TService, TOptions> : DriverJsonMapper<TBuilder, TService, TOptions>
    where TBuilder : DriverAtataContextBuilder<TBuilder, TService, TOptions>
    where TService : ChromiumDriverService
    where TOptions : ChromiumOptions, new()
{
    protected override void MapOptions(DriverOptionsJsonSection section, TOptions options)
    {
        base.MapOptions(section, options);

        if (section.Arguments?.Any() ?? false)
            options.AddArguments(section.Arguments);

        if (section.ExcludedArguments?.Any() ?? false)
            options.AddExcludedArguments(section.ExcludedArguments);

        if (section.Extensions?.Any() ?? false)
            options.AddExtensions(section.Extensions);

        if (section.EncodedExtensions?.Any() ?? false)
            options.AddEncodedExtensions(section.EncodedExtensions);

        if (section.WindowTypes?.Any() ?? false)
            options.AddWindowTypes(section.WindowTypes);

        if (section.PerformanceLoggingPreferences != null)
        {
            options.PerformanceLoggingPreferences = new ChromiumPerformanceLoggingPreferences();
            MapPerformanceLoggingPreferences(section.PerformanceLoggingPreferences, options.PerformanceLoggingPreferences);
        }

        if (section.UserProfilePreferences != null)
        {
            foreach (var item in section.UserProfilePreferences.ExtraPropertiesMap)
                options.AddUserProfilePreference(item.Key, FillTemplateVariables(item.Value));
        }

        if (section.LocalStatePreferences != null)
        {
            foreach (var item in section.LocalStatePreferences.ExtraPropertiesMap)
                options.AddLocalStatePreference(item.Key, FillTemplateVariables(item.Value));
        }

        if (!string.IsNullOrWhiteSpace(section.MobileEmulationDeviceName))
            options.EnableMobileEmulation(section.MobileEmulationDeviceName);

        if (section.MobileEmulationDeviceSettings != null)
            options.EnableMobileEmulation(section.MobileEmulationDeviceSettings);

        if (section.AndroidOptions != null)
            options.AndroidOptions = CreateAndMapAndroidOptions(section.AndroidOptions);
    }

    private void MapPerformanceLoggingPreferences(DriverPerformanceLoggingPreferencesJsonSection section, ChromiumPerformanceLoggingPreferences preferences)
    {
        ObjectMapper.Map(section.ExtraPropertiesMap, preferences);

        if (section.TracingCategories?.Any() ?? false)
            preferences.AddTracingCategories(section.TracingCategories);
    }

    private ChromiumAndroidOptions CreateAndMapAndroidOptions(AndroidOptionsJsonSection section)
    {
        if (string.IsNullOrEmpty(section.AndroidPackage))
            throw new ConfigurationException(
                "\"androidPackage\" configuration property of \"androidOptions\" section is not specified.");

        var androidOptions = new ChromiumAndroidOptions(FillTemplateVariables(section.AndroidPackage));
        ObjectMapper.Map(section.ExtraPropertiesMap, androidOptions);

        return androidOptions;
    }
}
