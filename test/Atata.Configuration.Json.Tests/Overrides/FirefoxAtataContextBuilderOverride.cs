using System;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;

namespace Atata.Configuration.Json.Tests
{
    using TOptions = FirefoxOptions;
    using TService = FirefoxDriverService;

    public class FirefoxAtataContextBuilderOverride : FirefoxAtataContextBuilder
    {
        [ThreadStatic]
        private static DriverContext<TService, TOptions> context;

        public FirefoxAtataContextBuilderOverride(AtataBuildingContext buildingContext)
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
