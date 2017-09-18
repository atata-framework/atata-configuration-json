using System.Linq;
using OpenQA.Selenium.Opera;

namespace Atata.Configuration.Json
{
    public class OperaDriverJsonMapper : DriverJsonMapper<OperaAtataContextBuilder, OperaDriverService, OperaOptions>
    {
        protected override OperaAtataContextBuilder CreateDriverBuilder(AtataContextBuilder builder)
        {
            return builder.UseOpera();
        }

        protected override void MapOptions(DriverOptionsJsonSection section, OperaOptions options)
        {
            base.MapOptions(section, options);

            if (section.GlobalAdditionalCapabilities != null)
            {
                foreach (var item in section.GlobalAdditionalCapabilities.ExtraPropertiesMap)
                    options.AddAdditionalCapability(item.Key, item.Value, true);
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

            if (section.UserProfilePreferences != null)
            {
                foreach (var item in section.UserProfilePreferences.ExtraPropertiesMap)
                    options.AddUserProfilePreference(item.Key, item.Value);
            }

            if (section.LocalStatePreferences != null)
            {
                foreach (var item in section.LocalStatePreferences.ExtraPropertiesMap)
                    options.AddLocalStatePreference(item.Key, item.Value);
            }
        }
    }
}
