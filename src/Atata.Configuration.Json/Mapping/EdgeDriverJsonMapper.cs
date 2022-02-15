using OpenQA.Selenium.Edge;

namespace Atata.Configuration.Json
{
    public class EdgeDriverJsonMapper : ChromiumDriverJsonMapper<EdgeAtataContextBuilder, EdgeDriverService, EdgeOptions>
    {
        protected override EdgeAtataContextBuilder CreateDriverBuilder(AtataContextBuilder builder) =>
            builder.UseEdge();

        protected override void MapOptions(DriverOptionsJsonSection section, EdgeOptions options)
        {
            base.MapOptions(section, options);

            if (section.AdditionalBrowserOptions != null)
            {
                foreach (var item in section.AdditionalBrowserOptions.ExtraPropertiesMap)
                    options.AddAdditionalEdgeOption(item.Key, FillTemplateVariables(item.Value));
            }
        }
    }
}
