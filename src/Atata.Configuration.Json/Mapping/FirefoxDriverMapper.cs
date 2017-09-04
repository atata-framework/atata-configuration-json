using System.Linq;
using OpenQA.Selenium.Firefox;

namespace Atata
{
    public class FirefoxDriverMapper : DriverMapper<FirefoxAtataContextBuilder, FirefoxDriverService, FirefoxOptions>
    {
        protected override FirefoxAtataContextBuilder CreateDriverBuilder(AtataContextBuilder builder)
        {
            return builder.UseFirefox();
        }

        protected override void MapOptions(DriverOptionsJsonSection section, FirefoxOptions options)
        {
            base.MapOptions(section, options);

            if (section.GlobalAdditionalCapabilities != null)
            {
                foreach (var item in section.GlobalAdditionalCapabilities.ExtraPropertiesMap)
                    options.AddAdditionalCapability(item.Key, item.Value, true);
            }

            if (section.Arguments?.Any() ?? false)
                options.AddArguments(section.Arguments);

            if (section.Profile != null)
            {
                options.Profile = options.Profile ?? new FirefoxProfile();

                AtataMapper.Map(section.Profile.ExtraPropertiesMap, options.Profile);

                if (section.Profile.Extensions != null)
                {
                    foreach (var item in section.Profile.Extensions)
                        options.Profile.AddExtension(item);
                }

                if (section.Profile.Preferences != null)
                {
                    foreach (var item in section.Profile.Preferences.ExtraPropertiesMap)
                        ; // TODO: Implement.
                }
            }
        }
    }
}
