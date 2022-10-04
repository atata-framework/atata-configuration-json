using OpenQA.Selenium;

namespace Atata.Configuration.Json.Tests;

using TOptions = OpenQA.Selenium.Chrome.ChromeOptions;
using TService = OpenQA.Selenium.Chrome.ChromeDriverService;

public class ChromeAtataContextBuilderOverride : ChromeAtataContextBuilder
{
    [ThreadStatic]
    private static DriverContext<TService, TOptions> s_context;

    public ChromeAtataContextBuilderOverride(AtataBuildingContext buildingContext)
        : base(buildingContext)
    {
    }

    public static DriverContext<TService, TOptions> Context =>
        s_context ??= new DriverContext<TService, TOptions>();

    protected override IWebDriver CreateDriver(TService service, TOptions options, TimeSpan commandTimeout)
    {
        Context.Set(service, options, commandTimeout);

        return Context.ReturnsNull ? null : base.CreateDriver(service, options, commandTimeout);
    }
}
