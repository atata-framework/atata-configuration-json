using System;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Remote;

namespace Atata.Configuration.Json.Tests
{
    using TOptions = PhantomJSOptions;
    using TService = PhantomJSDriverService;

    public class PhantomJSAtataContextBuilderOverride : PhantomJSAtataContextBuilder
    {
        [ThreadStatic]
        private static DriverContext<TService, TOptions> context;

        public PhantomJSAtataContextBuilderOverride(AtataBuildingContext buildingContext)
            : base(buildingContext)
        {
        }

        public static DriverContext<TService, TOptions> Context =>
            context ?? (context = new DriverContext<TService, TOptions>());

        protected override RemoteWebDriver CreateDriver(TService service, TOptions options, TimeSpan commandTimeout)
        {
            Context.Set(service, options, commandTimeout);

            return Context.ReturnsNull ? null : base.CreateDriver(service, options, commandTimeout);
        }
    }
}
