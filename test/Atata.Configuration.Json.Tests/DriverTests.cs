using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;

namespace Atata.Configuration.Json.Tests;

[TestFixture]
public class DriverTests : TestFixture
{
    [Test]
    public void Chrome()
    {
        var context = ChromeAtataContextBuilderOverride.Context;

        using (context.UseNullDriver())
        {
            AtataContextBuilder builder = AtataContext.Configure()
                .ApplyJsonConfig(@"Configs/Chrome.json");

            builder.BuildingContext.DriverFactoryToUse.Create();
        }

        VerifyChromeOptions(context.Options);
        VerifyChromeService(context.Service);

        context.CommandTimeout.Should().Be(TimeSpan.FromMinutes(1));
    }

    [Test]
    public void Chrome_ThruGlobalConfiguration()
    {
        AtataContext.GlobalConfiguration
            .ApplyJsonConfig(@"Configs/Chrome.json");

        var context = ChromeAtataContextBuilderOverride.Context;

        using (context.UseNullDriver())
        {
            AtataContextBuilder builder = AtataContext.Configure();

            builder.BuildingContext.DriverFactoryToUse.Create();
        }

        VerifyChromeOptions(context.Options);
        VerifyChromeService(context.Service);

        context.CommandTimeout.Should().Be(TimeSpan.FromMinutes(1));
    }

    [Test]
    public void Chrome_ThruGlobalConfiguration_AfterRuntimeConfiguration()
    {
        AtataContext.GlobalConfiguration
            .UseChrome()
                .WithCommandTimeout(TimeSpan.FromMinutes(2))
            .ApplyJsonConfig(@"Configs/Chrome.json");

        var context = ChromeAtataContextBuilderOverride.Context;

        using (context.UseNullDriver())
        {
            AtataContextBuilder builder = AtataContext.Configure();

            builder.BuildingContext.DriverFactoryToUse.Create();
        }

        VerifyChromeOptions(context.Options);
        VerifyChromeService(context.Service);

        context.CommandTimeout.Should().Be(TimeSpan.FromMinutes(1));

        VerifyChromeJsonConfig(JsonConfig.Global);
        JsonConfig.Current.Should().BeNull();
    }

    private static void VerifyChromeOptions(ChromeOptions options)
    {
        var capabilities = options.ToCapabilities();
        var browserCapabilities = (Dictionary<string, object>)capabilities.GetCapability(new ChromeOptions().CapabilityName);

        capabilities.GetCapability("goog:loggingPrefs").Should().BeEquivalentTo(new Dictionary<string, object>
        {
            ["browser"] = "INFO",
            ["driver"] = "WARNING"
        });

        browserCapabilities.Should().Contain(new Dictionary<string, object>
        {
            ["cap1"] = true,
            ["cap2"] = 5,
            ["cap3"] = "str"
        });

        capabilities.GetCapability("globalcap1").Should().Be(true);
        capabilities.GetCapability("globalcap2").Should().Be(5);
        capabilities.GetCapability("globalcap3").Should().Be("str");

        options.Proxy.Kind.Should().Be(ProxyKind.Manual);
        options.Proxy.HttpProxy.Should().Be("http");
        options.Proxy.FtpProxy.Should().Be("ftp");

        options.Arguments.Should().Equal("headless=new");

        ((List<string>)browserCapabilities["excludeSwitches"]).Should().Equal("exc-arg");

        options.Extensions.Should().Equal("ZW5jLWV4dDE=", "ZW5jLWV4dDI=");

        ((List<string>)browserCapabilities["windowTypes"]).Should().Equal("win1", "win2");

        options.PerformanceLoggingPreferences.IsCollectingNetworkEvents.Should().BeFalse();
        options.PerformanceLoggingPreferences.IsCollectingPageEvents.Should().BeFalse();
        options.PerformanceLoggingPreferences.BufferUsageReportingInterval.Should().Be(TimeSpan.FromSeconds(70));
        options.PerformanceLoggingPreferences.TracingCategories.Should().Be("cat1,cat2");

        ((Dictionary<string, object>)browserCapabilities["prefs"]).Should().Equal(new Dictionary<string, object>
        {
            ["pref1"] = 7,
            ["pref2"] = false,
            ["pref3"] = "str"
        });

        ((Dictionary<string, object>)browserCapabilities["localState"]).Should().Equal(new Dictionary<string, object>
        {
            ["pref1"] = 2.7,
            ["pref2"] = true,
            ["pref3"] = string.Empty
        });

        ((Dictionary<string, object>)browserCapabilities["mobileEmulation"]).Should().Equal(new Dictionary<string, object>
        {
            ["deviceName"] = "emul"
        });

        browserCapabilities["androidPackage"].Should().Be("pack1");
        browserCapabilities["androidActivity"].Should().Be("act1");

        options.LeaveBrowserRunning.Should().BeTrue();
        options.MinidumpPath.Should().Be("mdp");
    }

    private static void VerifyChromeService(ChromeDriverService service)
    {
        service.Port.Should().Be(555);
        service.HostName.Should().Be("127.0.0.1");
        service.WhitelistedIPAddresses.Should().Be("5.5.5.5,7.7.7.7");
    }

    [Test]
    public void Chrome_ThruGlobalConfiguration_VerifyJsonConfig()
    {
        AtataContext.GlobalConfiguration
            .ApplyJsonConfig(@"Configs/Chrome.json");

        AtataContext.Configure()
            .UseChrome()
                .WithArguments("headless=new")
            .Build();

        VerifyChromeJsonConfig(JsonConfig.Current);
        VerifyChromeJsonConfig(JsonConfig.Global);
    }

    private static void VerifyChromeJsonConfig(JsonConfig config)
    {
        using (new AssertionScope())
        {
            config.Driver.Options.LoggingPreferences.Should().Equal(
                new Dictionary<string, OpenQA.Selenium.LogLevel>
                {
                    ["browser"] = OpenQA.Selenium.LogLevel.Info,
                    ["driver"] = OpenQA.Selenium.LogLevel.Warning
                });

            config.Driver.Options.AdditionalOptions.ExtraPropertiesMap.Should().Equal(
                new Dictionary<string, object>
                {
                    ["globalcap1"] = true,
                    ["globalcap2"] = 5,
                    ["globalcap3"] = "str"
                });

            config.Driver.Options.AdditionalBrowserOptions.ExtraPropertiesMap.Should().BeEquivalentTo(
                new Dictionary<string, object>
                {
                    ["cap1"] = true,
                    ["cap2"] = 5,
                    ["cap3"] = "str",
                    ["cap4"] = new Dictionary<string, object>
                    {
                        ["cap4:1"] = false,
                        ["cap4:2"] = 14,
                        ["cap4:3"] = new Dictionary<string, object>
                        {
                            ["cap4:3:1"] = "str2"
                        }
                    }
                });

            config.Driver.Options.Proxy.ExtraPropertiesMap.Should().Equal(
               new Dictionary<string, object>
               {
                   ["httpProxy"] = "http",
                   ["ftpProxy"] = "ftp"
               });

            config.Driver.Options.Arguments.Should().Equal("headless=new");
            config.Driver.Options.ExcludedArguments.Should().Equal("exc-arg");

            config.Driver.Options.EncodedExtensions.Should().Equal("ZW5jLWV4dDE=", "ZW5jLWV4dDI=");

            config.Driver.Options.WindowTypes.Should().Equal("win1", "win2");

            config.Driver.Options.PerformanceLoggingPreferences.ExtraPropertiesMap.Should().Equal(
                new Dictionary<string, object>
                {
                    ["isCollectingNetworkEvents"] = false,
                    ["IsCollectingPageEvents"] = false,
                    ["bufferUsageReportingInterval"] = "00:01:10"
                });
            config.Driver.Options.PerformanceLoggingPreferences.TracingCategories.Should().Equal("cat1", "cat2");

            config.Driver.Options.UserProfilePreferences.ExtraPropertiesMap.Should().Equal(
                new Dictionary<string, object>
                {
                    ["pref1"] = 7,
                    ["pref2"] = false,
                    ["pref3"] = "str"
                });

            config.Driver.Options.LocalStatePreferences.ExtraPropertiesMap.Should().Equal(
                new Dictionary<string, object>
                {
                    ["pref1"] = 2.7,
                    ["pref2"] = true,
                    ["pref3"] = string.Empty
                });

            config.Driver.Options.MobileEmulationDeviceName.Should().Be("emul");

            config.Driver.Options.ExtraPropertiesMap.Should().Equal(
                new Dictionary<string, object>
                {
                    ["LeaveBrowserRunning"] = true,
                    ["minidumpPath"] = "mdp"
                });

            config.Driver.Options.AndroidOptions.AndroidPackage.Should().Be("pack1");
            config.Driver.Options.AndroidOptions.ExtraPropertiesMap.Should().Equal(
                new Dictionary<string, object>
                {
                    ["androidActivity"] = "act1",
                });

            config.Driver.Service.ExtraPropertiesMap.Should().Equal(
                new Dictionary<string, object>
                {
                    ["port"] = 555,
                    ["hostName"] = "127.0.0.1",
                    ["whitelistedIPAddresses"] = "5.5.5.5,7.7.7.7"
                });
        }
    }

    [Test]
    public void Firefox()
    {
        var context = FirefoxAtataContextBuilderOverride.Context;

        using (context.UseNullDriver())
        {
            AtataContextBuilder builder = AtataContext.Configure()
                .ApplyJsonConfig(@"Configs/Firefox.json");

            builder.BuildingContext.DriverFactoryToUse.Create();
        }

        var capabilities = context.Options.ToCapabilities();
        var optionsCapabilities = (Dictionary<string, object>)capabilities.GetCapability("moz:firefoxOptions");

        optionsCapabilities.Should().Contain(new Dictionary<string, object>
        {
            ["cap1"] = false,
            ["cap2"] = 15,
            ["cap3"] = "str2"
        });

        capabilities.GetCapability("globalcap1").Should().Be(true);
        capabilities.GetCapability("globalcap2").Should().Be(5);
        capabilities.GetCapability("globalcap3").Should().Be("str");

        context.Options.Proxy.Kind.Should().Be(ProxyKind.ProxyAutoConfigure);

        ((List<object>)optionsCapabilities["args"]).Should().Equal("--start-maximized");

        ((Dictionary<string, object>)optionsCapabilities["prefs"]).Should().Equal(new Dictionary<string, object>
        {
            ["pref1"] = true,
            ["pref2"] = 5,
            ["pref3"] = "str"
        });

        context.Options.LogLevel.Should().Be(FirefoxDriverLogLevel.Warn);

        optionsCapabilities["androidPackage"].Should().Be("pack1");
        optionsCapabilities["androidActivity"].Should().Be("act1");
        optionsCapabilities["androidIntentArguments"].Should().BeEquivalentTo(new[] { "arg1", "arg2" });

        context.Service.Host.Should().Be("127.0.0.5");

        context.CommandTimeout.Should().Be(TimeSpan.FromSeconds(0.95));
    }

    [Test]
    public void InternetExplorer()
    {
        var context = InternetExplorerAtataContextBuilderOverride.Context;

        using (context.UseNullDriver())
        {
            AtataContextBuilder builder = AtataContext.Configure()
                .ApplyJsonConfig(@"Configs/InternetExplorer.json");

            builder.BuildingContext.DriverFactoryToUse.Create();
        }

        var capabilities = context.Options.ToCapabilities();
        var optionsCapabilities = (Dictionary<string, object>)capabilities.GetCapability(InternetExplorerOptions.Capability);

        optionsCapabilities.Should().Contain(new Dictionary<string, object>
        {
            ["cap1"] = false,
            ["cap2"] = 15,
            ["cap3"] = "str2"
        });

        capabilities.GetCapability("globalcap1").Should().Be(true);
        capabilities.GetCapability("globalcap2").Should().Be(5);
        capabilities.GetCapability("globalcap3").Should().Be("str");

        context.Options.Proxy.Kind.Should().Be(ProxyKind.Manual);
        context.Options.Proxy.SocksVersion.Should().Be(5);
        context.Options.Proxy.SocksProxy.Should().Be("socks");
        context.Options.Proxy.SocksUserName.Should().Be("name");
        context.Options.Proxy.SocksPassword.Should().Be("pass");

        context.Options.EnableNativeEvents.Should().BeFalse();
        context.Options.RequireWindowFocus.Should().BeTrue();
        context.Options.BrowserAttachTimeout.Should().Be(TimeSpan.FromSeconds(2));
        context.Options.ElementScrollBehavior.Should().Be(InternetExplorerElementScrollBehavior.Bottom);

        context.Service.LoggingLevel.Should().Be(InternetExplorerDriverLogLevel.Debug);

        context.CommandTimeout.Should().Be(TimeSpan.FromSeconds(45));
    }

    [Test]
    public void Edge()
    {
        var context = EdgeAtataContextBuilderOverride.Context;

        using (context.UseNullDriver())
        {
            AtataContextBuilder builder = AtataContext.Configure()
                .ApplyJsonConfig(@"Configs/Edge.json");

            builder.BuildingContext.DriverFactoryToUse.Create();
        }

        var capabilities = context.Options.ToCapabilities();

        capabilities.GetCapability("cap1").Should().Be(true);
        capabilities.GetCapability("cap2").Should().Be(5);
        capabilities.GetCapability("cap3").Should().Be("str");

        context.Options.PageLoadStrategy.Should().Be(PageLoadStrategy.Eager);

        context.Service.WhitelistedIPAddresses.Should().Be("ips");
    }

    [Test]
    public void Remote()
    {
        var context = RemoteDriverAtataContextBuilderOverride.Context;

        using (context.UseNullDriver())
        {
            AtataContextBuilder builder = AtataContext.Configure()
                .ApplyJsonConfig(@"Configs/Remote.json");

            builder.BuildingContext.DriverFactoryToUse.Create();
        }

        context.RemoteAddress.Should().Be("http://127.0.0.1:8888/wd/hub");
        context.CommandTimeout.Should().Be(TimeSpan.FromSeconds(100));
    }

    [Test]
    public void Remote_WithOptions()
    {
        var context = RemoteDriverAtataContextBuilderOverride.Context;

        using (context.UseNullDriver())
        {
            AtataContextBuilder builder = AtataContext.Configure()
                .ApplyJsonConfig(@"Configs/RemoteChrome.json");

            builder.BuildingContext.DriverFactoryToUse.Create();
        }

        ICapabilities capabilities = context.Capabilities;

        capabilities.GetCapability(CapabilityType.BrowserName).Should().Be(DriverAliases.Chrome);

        var chromeCapabilities = (Dictionary<string, object>)capabilities.GetCapability(new ChromeOptions().CapabilityName);

        chromeCapabilities.Should().Equal(new Dictionary<string, object>
        {
            ["detach"] = true,
            ["cap1"] = true,
            ["cap2"] = 5,
            ["cap3"] = "str"
        });
    }

    [Test]
    public void Remote_WithTypelessOptions()
    {
        var context = RemoteDriverAtataContextBuilderOverride.Context;

        using (context.UseNullDriver())
        {
            AtataContextBuilder builder = AtataContext.Configure()
                .ApplyJsonConfig(@"Configs/RemoteTypeless.json");

            Assert.Throws<ArgumentNullException>(() =>
                builder.BuildingContext.DriverFactoryToUse.Create());
        }
    }

    [Test]
    public void Remote_WithoutType()
    {
        var context = RemoteDriverAtataContextBuilderOverride.Context;

        using (context.UseNullDriver())
        {
            AtataContextBuilder builder = AtataContext.Configure()
                .ApplyJsonConfig(@"Configs/RemoteFirefox.json");

            builder.BuildingContext.DriverFactoryToUse.Create();
        }

        ICapabilities capabilities = context.Capabilities;

        capabilities.GetCapability(CapabilityType.BrowserName).Should().Be(DriverAliases.Firefox);
    }

    [Test]
    public void Multiple()
    {
        AtataContextBuilder builder = AtataContext.Configure()
            .ApplyJsonConfig(@"Configs/MultipleDrivers.json");

        builder.BuildingContext.DriverFactories.Should().HaveCount(2);
        builder.BuildingContext.DriverFactories[0].Alias.Should().Be(DriverAliases.Firefox);
        builder.BuildingContext.DriverFactories[1].Alias.Should().Be(DriverAliases.Chrome);
        builder.BuildingContext.DriverFactoryToUse.Alias.Should().Be(DriverAliases.Chrome);
    }

    [Test]
    public void Multiple_ThruGlobalConfiguration()
    {
        AtataContext.GlobalConfiguration
            .ApplyJsonConfig(@"Configs/MultipleDrivers.json");

        JsonConfig.Global.Drivers.Select(x => x.Type).Should().Equal(DriverAliases.Firefox, DriverAliases.Chrome);

        AtataContextBuilder builder = AtataContext.Configure();

        builder.BuildingContext.DriverFactories.Should().HaveCount(2);
        builder.BuildingContext.DriverFactories[0].Alias.Should().Be(DriverAliases.Firefox);
        builder.BuildingContext.DriverFactories[1].Alias.Should().Be(DriverAliases.Chrome);
        builder.BuildingContext.DriverFactoryToUse.Alias.Should().Be(DriverAliases.Chrome);

        builder.Build();
        AtataContext.Current.Driver.Should().BeOfType<ChromeDriver>();
        JsonConfig.Global.Drivers.Should().HaveCount(2);
        JsonConfig.Current.Drivers.Should().HaveCount(2);
    }

    [Test]
    public void Multiple_ViaMultipleConfigs()
    {
        AtataContextBuilder builder = AtataContext.Configure()
            .ApplyJsonConfig(@"Configs/Edge.json")
            .ApplyJsonConfig(@"Configs/Firefox.json")
            .ApplyJsonConfig(@"Configs/Chrome.json");

        builder.BuildingContext.DriverFactories.Should().HaveCount(3);

        using (new AssertionScope())
        {
            builder.BuildingContext.DriverFactories[0].Alias.Should().Be(DriverAliases.Edge);
            builder.BuildingContext.DriverFactories[1].Alias.Should().Be(DriverAliases.Firefox);
            builder.BuildingContext.DriverFactories[2].Alias.Should().Be(DriverAliases.Chrome);
            builder.BuildingContext.DriverFactoryToUse.Alias.Should().Be(DriverAliases.Chrome);
        }

        JsonConfig.Current.Drivers.Should().HaveCount(3);
        VerifyChromeJsonConfig(JsonConfig.Current);
    }
}
