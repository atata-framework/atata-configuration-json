using NUnit.Framework;

namespace Atata.Configuration.Json.Tests
{
    [SetUpFixture]
    public class SetUpFixture
    {
        [OneTimeSetUp]
        public void SetUp()
        {
            DriverJsonMapperAliases.Register<RemoteDriverJsonMapperOverride>(DriverJsonMapperAliases.Remote);
            DriverJsonMapperAliases.Register<ChromeDriverJsonMapperOverride>(DriverJsonMapperAliases.Chrome);
            DriverJsonMapperAliases.Register<FirefoxDriverJsonMapperOverride>(DriverJsonMapperAliases.Firefox);
            DriverJsonMapperAliases.Register<InternetExplorerDriverJsonMapperOverride>(DriverJsonMapperAliases.InternetExplorer);
            DriverJsonMapperAliases.Register<EdgeDriverJsonMapperOverride>(DriverJsonMapperAliases.Edge);
            DriverJsonMapperAliases.Register<PhantomJSDriverJsonMapperOverride>(DriverJsonMapperAliases.PhantomJS);
        }
    }
}
