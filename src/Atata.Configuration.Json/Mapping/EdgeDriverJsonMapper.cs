using OpenQA.Selenium.Edge;

namespace Atata.Configuration.Json
{
    public class EdgeDriverJsonMapper : DriverJsonMapper<EdgeAtataContextBuilder, EdgeDriverService, EdgeOptions>
    {
        protected override EdgeAtataContextBuilder CreateDriverBuilder(AtataContextBuilder builder)
        {
            return builder.UseEdge();
        }
    }
}
