using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace Atata.Configuration.Json.Tests
{
    public class RemoteDriverAtataContextBuilderOverride : RemoteDriverAtataContextBuilder
    {
        [ThreadStatic]
        private static RemoteDriverContext context;

        public RemoteDriverAtataContextBuilderOverride(AtataBuildingContext buildingContext)
            : base(buildingContext)
        {
        }

        public static RemoteDriverContext Context =>
            context ?? (context = new RemoteDriverContext());

        protected override RemoteWebDriver CreateDriver(Uri remoteAddress, ICapabilities capabilities, TimeSpan commandTimeout)
        {
            Context.Set(remoteAddress, capabilities, commandTimeout);

            return Context.ReturnsNull ? null : base.CreateDriver(remoteAddress, capabilities, commandTimeout);
        }
    }
}
