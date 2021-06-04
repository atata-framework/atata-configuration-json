using System.Linq;
using OpenQA.Selenium.Chrome;

namespace Atata.Configuration.Json
{
    public class ChromeDriverJsonMapper : DriverJsonMapper<ChromeAtataContextBuilder, ChromeDriverService, ChromeOptions>
    {
        protected override ChromeAtataContextBuilder CreateDriverBuilder(AtataContextBuilder builder)
        {
            return builder.UseChrome();
        }

        protected override void MapOptions(DriverOptionsJsonSection section, ChromeOptions options)
        {
            base.MapOptions(section, options);

            if (section.GlobalAdditionalCapabilities != null)
            {
                foreach (var item in section.GlobalAdditionalCapabilities.ExtraPropertiesMap)
                    options.AddAdditionalCapability(item.Key, FillTemplateVariables(item.Value), true);
            }

            if (section.Proxy != null)
                options.Proxy = section.Proxy.ToProxy();

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
                options.PerformanceLoggingPreferences = new ChromePerformanceLoggingPreferences();
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
        }

        private void MapPerformanceLoggingPreferences(DriverPerformanceLoggingPreferencesJsonSection section, ChromePerformanceLoggingPreferences preferences)
        {
            ObjectMapper.Map(section.ExtraPropertiesMap, preferences);

            if (section.TracingCategories?.Any() ?? false)
                preferences.AddTracingCategories(section.TracingCategories);
        }
    }
}
