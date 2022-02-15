using OpenQA.Selenium.Chrome;

namespace Atata.Configuration.Json
{
    public class ChromeDriverJsonMapper : ChromiumDriverJsonMapper<ChromeAtataContextBuilder, ChromeDriverService, ChromeOptions>
    {
        protected override ChromeAtataContextBuilder CreateDriverBuilder(AtataContextBuilder builder) =>
            builder.UseChrome();

        protected override void MapOptions(DriverOptionsJsonSection section, ChromeOptions options)
        {
            base.MapOptions(section, options);

            if (section.AdditionalBrowserOptions != null)
            {
                foreach (var item in section.AdditionalBrowserOptions.ExtraPropertiesMap)
                    options.AddAdditionalChromeOption(item.Key, FillTemplateVariables(item.Value));
            }
        }
    }
}
