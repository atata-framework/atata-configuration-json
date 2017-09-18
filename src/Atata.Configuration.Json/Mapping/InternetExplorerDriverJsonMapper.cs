using OpenQA.Selenium.IE;

namespace Atata.Configuration.Json
{
    public class InternetExplorerDriverJsonMapper : DriverJsonMapper<InternetExplorerAtataContextBuilder, InternetExplorerDriverService, InternetExplorerOptions>
    {
        protected override InternetExplorerAtataContextBuilder CreateDriverBuilder(AtataContextBuilder builder)
        {
            return builder.UseInternetExplorer();
        }

        protected override void MapOptions(DriverOptionsJsonSection section, InternetExplorerOptions options)
        {
            base.MapOptions(section, options);

            if (section.GlobalAdditionalCapabilities != null)
            {
                foreach (var item in section.GlobalAdditionalCapabilities.ExtraPropertiesMap)
                    options.AddAdditionalCapability(item.Key, item.Value, true);
            }

            if (section.Proxy != null)
                options.Proxy = section.Proxy.ToProxy();
        }
    }
}
