using System.Linq;
using OpenQA.Selenium.Edge;

namespace Atata.Configuration.Json
{
    public class EdgeDriverJsonMapper : DriverJsonMapper<EdgeAtataContextBuilder, EdgeDriverService, EdgeOptions>
    {
        protected override EdgeAtataContextBuilder CreateDriverBuilder(AtataContextBuilder builder)
        {
            return builder.UseEdge();
        }

        protected override void MapOptions(DriverOptionsJsonSection section, EdgeOptions options)
        {
            base.MapOptions(section, options);

            if (section.Extensions?.Any() ?? false)
                options.AddExtensionPaths(section.Extensions);
        }
    }
}
