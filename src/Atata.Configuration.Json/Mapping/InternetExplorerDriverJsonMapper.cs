using OpenQA.Selenium.IE;

namespace Atata.Configuration.Json;

public class InternetExplorerDriverJsonMapper : DriverJsonMapper<InternetExplorerAtataContextBuilder, InternetExplorerDriverService, InternetExplorerOptions>
{
    protected override InternetExplorerAtataContextBuilder CreateDriverBuilder(AtataContextBuilder builder) =>
        builder.UseInternetExplorer();

    protected override void MapOptions(DriverOptionsJsonSection section, InternetExplorerOptions options)
    {
        base.MapOptions(section, options);

        if (section.AdditionalBrowserOptions != null)
        {
            foreach (var item in section.AdditionalBrowserOptions.ExtraPropertiesMap)
                options.AddAdditionalInternetExplorerOption(item.Key, FillTemplateVariables(item.Value));
        }
    }
}
