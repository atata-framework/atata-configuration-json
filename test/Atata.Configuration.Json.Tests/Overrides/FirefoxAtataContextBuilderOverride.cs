using System;
using OpenQA.Selenium.Remote;

namespace Atata.Configuration.Json.Tests
{
    using TOptions = OpenQA.Selenium.Firefox.FirefoxOptions;
    using TService = OpenQA.Selenium.Firefox.FirefoxDriverService;

    public class FirefoxAtataContextBuilderOverride : FirefoxAtataContextBuilder
    {
        [ThreadStatic]
        private static DriverContext<TService, TOptions> s_context;

        public FirefoxAtataContextBuilderOverride(AtataBuildingContext buildingContext)
            : base(buildingContext)
        {
        }

        public static DriverContext<TService, TOptions> Context =>
            s_context ??= new DriverContext<TService, TOptions>();

        protected override RemoteWebDriver CreateDriver(TService service, TOptions options, TimeSpan commandTimeout)
        {
            Context.Set(service, options, commandTimeout);

            return Context.ReturnsNull ? null : base.CreateDriver(service, options, commandTimeout);
        }
    }
}
