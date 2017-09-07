using NUnit.Framework;

namespace Atata.Configuration.Json.Tests
{
    [SetUpFixture]
    public class SetUpFixture
    {
        [OneTimeSetUp]
        public void SetUp()
        {
            DriverJsonMapperAliases.Register<ChromeDriverJsonMapperOverride>(DriverJsonMapperAliases.Chrome);
            DriverJsonMapperAliases.Register<FirefoxDriverJsonMapperOverride>(DriverJsonMapperAliases.Firefox);
            DriverJsonMapperAliases.Register<InternetExplorerDriverJsonMapperOverride>(DriverJsonMapperAliases.InternetExplorer);
        }
    }
}
