using Atata.WebDriverSetup;

namespace Atata.Configuration.Json.Tests;

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

        DriverSetup.AutoSetUp(BrowserNames.Chrome, BrowserNames.Firefox, BrowserNames.Edge, BrowserNames.InternetExplorer);
    }
}
