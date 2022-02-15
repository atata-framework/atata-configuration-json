using System;
using OpenQA.Selenium;

namespace Atata.Configuration.Json.Tests
{
    public class RemoteDriverAtataContextBuilderOverride : RemoteDriverAtataContextBuilder
    {
        [ThreadStatic]
        private static RemoteDriverContext s_context;

        public RemoteDriverAtataContextBuilderOverride(AtataBuildingContext buildingContext)
            : base(buildingContext)
        {
        }

        public static RemoteDriverContext Context =>
            s_context ??= new RemoteDriverContext();

        protected override IWebDriver CreateDriver(Uri remoteAddress, ICapabilities capabilities, TimeSpan commandTimeout)
        {
            Context.Set(remoteAddress, capabilities, commandTimeout);

            return Context.ReturnsNull ? null : base.CreateDriver(remoteAddress, capabilities, commandTimeout);
        }
    }
}
