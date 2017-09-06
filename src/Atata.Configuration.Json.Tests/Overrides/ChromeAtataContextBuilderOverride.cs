using System;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace Atata.Configuration.Json.Tests
{
    public class ChromeAtataContextBuilderOverride : ChromeAtataContextBuilder
    {
        [ThreadStatic]
        private static DriverContext<ChromeDriverService, ChromeOptions> context;

        public ChromeAtataContextBuilderOverride(AtataBuildingContext buildingContext)
            : base(buildingContext)
        {
        }

        public static DriverContext<ChromeDriverService, ChromeOptions> Context =>
            context ?? (context = new DriverContext<ChromeDriverService, ChromeOptions>());

        protected override RemoteWebDriver CreateDriver(ChromeDriverService service, ChromeOptions options, TimeSpan commandTimeout)
        {
            Context.Set(service, options, commandTimeout);

            return Context.ReturnsNull ? null : base.CreateDriver(service, options, commandTimeout);
        }
    }
}
