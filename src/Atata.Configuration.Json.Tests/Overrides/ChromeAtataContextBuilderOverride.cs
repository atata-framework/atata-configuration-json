using System;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace Atata.Configuration.Json.Tests
{
    using TOptions = ChromeOptions;
    using TService = ChromeDriverService;

    public class ChromeAtataContextBuilderOverride : ChromeAtataContextBuilder
    {
        [ThreadStatic]
        private static DriverContext<TService, TOptions> context;

        public ChromeAtataContextBuilderOverride(AtataBuildingContext buildingContext)
            : base(buildingContext)
        {
        }

        public static DriverContext<TService, TOptions> Context =>
            context ??= new DriverContext<TService, TOptions>();

        protected override RemoteWebDriver CreateDriver(TService service, TOptions options, TimeSpan commandTimeout)
        {
            Context.Set(service, options, commandTimeout);

            return Context.ReturnsNull ? null : base.CreateDriver(service, options, commandTimeout);
        }
    }
}
