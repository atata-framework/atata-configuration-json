using NUnit.Framework;

namespace Atata.Configuration.Json.Tests
{
    [SetUpFixture]
    public class SetUpFixture
    {
        [OneTimeSetUp]
        public void SetUp()
        {
            DriverJsonMapperAliases.Register<RemoteDriverJsonMapperOverride>(DriverAliases.Remote);
            DriverJsonMapperAliases.Register<ChromeDriverJsonMapperOverride>(DriverAliases.Chrome);
            DriverJsonMapperAliases.Register<FirefoxDriverJsonMapperOverride>(DriverAliases.Firefox);
            DriverJsonMapperAliases.Register<InternetExplorerDriverJsonMapperOverride>(DriverAliases.InternetExplorer);
            DriverJsonMapperAliases.Register<EdgeDriverJsonMapperOverride>(DriverAliases.Edge);
            DriverJsonMapperAliases.Register<PhantomJSDriverJsonMapperOverride>(DriverAliases.PhantomJS);
        }
    }
}
