using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;

namespace Atata.Configuration.Json.Tests
{
    [TestFixture]
    public class DriverTests : TestFixture
    {
        [Test]
        public void Driver_Chrome()
        {
            var context = ChromeAtataContextBuilderOverride.Context;

            using (context.UseNullDriver())
            {
                AtataContextBuilder builder = AtataContext.Configure().
                    ApplyJsonConfig(@"Configs/Chrome.json");

                builder.BuildingContext.DriverFactoryToUse.Create();
            }

            VerifyChromeOptions(context.Options);
            VerifyChromeService(context.Service);

            context.CommandTimeout.Should().Be(TimeSpan.FromMinutes(1));
        }

        [Test]
        public void Driver_Chrome_ThruGlobalConfiguration()
        {
            AtataContext.GlobalConfiguration.
                ApplyJsonConfig(@"Configs/Chrome.json");

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

        private static void VerifyChromeOptions(ChromeOptions options)
        {
            var capabilities = options.ToCapabilities();
            var optionsCapabilities = (Dictionary<string, object>)capabilities.GetCapability(ChromeOptions.Capability);

            capabilities.GetCapability(CapabilityType.LoggingPreferences).ShouldBeEquivalentTo(new Dictionary<string, object>
            {
                ["browser"] = "INFO",
                ["driver"] = "WARNING"
            });

            optionsCapabilities.Should().Contain(new Dictionary<string, object>
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

            options.Arguments.Should().Equal("disable-extensions", "start-maximized");

            ((List<string>)optionsCapabilities["excludeSwitches"]).Should().Equal("exc-arg");

            options.Extensions.Should().Equal("ZW5jLWV4dDE=", "ZW5jLWV4dDI=");

            ((List<string>)optionsCapabilities["windowTypes"]).Should().Equal("win1", "win2");

            options.PerformanceLoggingPreferences.IsCollectingNetworkEvents.Should().BeFalse();
            options.PerformanceLoggingPreferences.IsCollectingPageEvents.Should().BeFalse();
            options.PerformanceLoggingPreferences.BufferUsageReportingInterval.Should().Be(TimeSpan.FromSeconds(70));
            options.PerformanceLoggingPreferences.TracingCategories.Should().Be("cat1,cat2");

            ((Dictionary<string, object>)optionsCapabilities["prefs"]).Should().Equal(new Dictionary<string, object>
            {
                ["pref1"] = 7,
                ["pref2"] = false,
                ["pref3"] = "str"
            });

            ((Dictionary<string, object>)optionsCapabilities["localState"]).Should().Equal(new Dictionary<string, object>
            {
                ["pref1"] = 2.7,
                ["pref2"] = true,
                ["pref3"] = string.Empty
            });

            ((Dictionary<string, object>)optionsCapabilities["mobileEmulation"]).Should().Equal(new Dictionary<string, object>
            {
                ["deviceName"] = "emul"
            });

            options.LeaveBrowserRunning.Should().BeTrue();
            options.MinidumpPath.Should().Be("mdp");
        }

        private void VerifyChromeService(ChromeDriverService service)
        {
            service.Port.Should().Be(555);
            service.HostName.Should().Be("127.0.0.1");
            service.WhitelistedIPAddresses.Should().Be("5.5.5.5,7.7.7.7");
        }

        [Test]
        public void Driver_Chrome_ThruGlobalConfiguration_VerifyJsonConfig()
        {
            AtataContext.GlobalConfiguration.
                ApplyJsonConfig(@"Configs/Chrome.json");

            AtataContext.Configure().
                UseChrome().
                    WithDriverService(() => ChromeDriverService.CreateDefaultService()).
                Build();

            VerifyChromeJsonConfig(JsonConfig.Current);
            VerifyChromeJsonConfig(JsonConfig.Global);
        }

        private static void VerifyChromeJsonConfig(JsonConfig config)
        {
            config.Driver.Options.LoggingPreferences.Should().Equal(
                            new Dictionary<string, OpenQA.Selenium.LogLevel>
                            {
                                ["browser"] = OpenQA.Selenium.LogLevel.Info,
                                ["driver"] = OpenQA.Selenium.LogLevel.Warning
                            });

            config.Driver.Options.AdditionalCapabilities.ExtraPropertiesMap.Should().Equal(
                new Dictionary<string, object>
                {
                    ["cap1"] = true,
                    ["cap2"] = 5,
                    ["cap3"] = "str"
                });

            config.Driver.Options.GlobalAdditionalCapabilities.ExtraPropertiesMap.Should().Equal(
                new Dictionary<string, object>
                {
                    ["globalcap1"] = true,
                    ["globalcap2"] = 5,
                    ["globalcap3"] = "str"
                });

            config.Driver.Options.Proxy.Kind.Should().BeNull();
            config.Driver.Options.Proxy.HttpProxy.Should().Be("http");
            config.Driver.Options.Proxy.FtpProxy.Should().Be("ftp");

            config.Driver.Options.Arguments.Should().Equal("disable-extensions", "start-maximized");
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

            config.Driver.Service.ExtraPropertiesMap.Should().Equal(
                new Dictionary<string, object>
                {
                    ["port"] = 555,
                    ["hostName"] = "127.0.0.1",
                    ["whitelistedIPAddresses"] = "5.5.5.5,7.7.7.7"
                });
        }

        [Test]
        public void Driver_Firefox()
        {
            var context = FirefoxAtataContextBuilderOverride.Context;

            using (context.UseNullDriver())
            {
                AtataContextBuilder builder = AtataContext.Configure().
                    ApplyJsonConfig(@"Configs/Firefox.json");

                builder.BuildingContext.DriverFactoryToUse.Create();
            }

            var capabilities = context.Options.ToCapabilities();
            var optionsCapabilities = (Dictionary<string, object>)capabilities.GetCapability("moz:firefoxOptions");

            optionsCapabilities.Should().Contain(new Dictionary<string, object>
            {
                ["cap1"] = true,
                ["cap2"] = 5,
                ["cap3"] = "str"
            });

            capabilities.GetCapability("globalcap1").Should().Be(true);
            capabilities.GetCapability("globalcap2").Should().Be(5);
            capabilities.GetCapability("globalcap3").Should().Be("str");

            context.Options.Proxy.Kind.Should().Be(ProxyKind.ProxyAutoConfigure);

            ((List<object>)optionsCapabilities["args"]).Should().Equal("--start-maximized");

            typeof(FirefoxProfile).GetField("sourceProfileDir", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.GetValue(context.Options.Profile).Should().Be("dir");

            typeof(FirefoxProfile).GetField("deleteSource", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.GetValue(context.Options.Profile).Should().Be(true);

            context.Options.Profile.EnableNativeEvents.Should().BeTrue();

            ((Dictionary<string, object>)optionsCapabilities["prefs"]).Should().Equal(new Dictionary<string, object>
            {
                ["pref1"] = true,
                ["pref2"] = 5,
                ["pref3"] = "str"
            });

            context.Options.LogLevel.Should().Be(FirefoxDriverLogLevel.Warn);

            context.Service.Host.Should().Be("127.0.0.5");

            context.CommandTimeout.Should().Be(TimeSpan.FromSeconds(0.95));
        }

        [Test]
        public void Driver_InternetExplorer()
        {
            var context = InternetExplorerAtataContextBuilderOverride.Context;

            using (context.UseNullDriver())
            {
                AtataContextBuilder builder = AtataContext.Configure().
                    ApplyJsonConfig(@"Configs/InternetExplorer.json");

                builder.BuildingContext.DriverFactoryToUse.Create();
            }

            var capabilities = context.Options.ToCapabilities();
            var optionsCapabilities = (Dictionary<string, object>)capabilities.GetCapability(InternetExplorerOptions.Capability);

            optionsCapabilities.Should().Contain(new Dictionary<string, object>
            {
                ["cap1"] = true,
                ["cap2"] = 5,
                ["cap3"] = "str"
            });

            capabilities.GetCapability("globalcap1").Should().Be(true);
            capabilities.GetCapability("globalcap2").Should().Be(5);
            capabilities.GetCapability("globalcap3").Should().Be("str");

            context.Options.Proxy.Kind.Should().Be(ProxyKind.Manual);
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
        public void Driver_Edge()
        {
            var context = EdgeAtataContextBuilderOverride.Context;

            using (context.UseNullDriver())
            {
                AtataContextBuilder builder = AtataContext.Configure().
                    ApplyJsonConfig(@"Configs/Edge.json");

                builder.BuildingContext.DriverFactoryToUse.Create();
            }

            var capabilities = context.Options.ToCapabilities();

            capabilities.GetCapability("cap1").Should().Be(true);
            capabilities.GetCapability("cap2").Should().Be(5);
            capabilities.GetCapability("cap3").Should().Be("str");

            context.Options.PageLoadStrategy.Should().Be(PageLoadStrategy.Eager);

            context.Service.Package.Should().Be("pack");
        }

        [Test]
        public void Driver_Remote()
        {
            var context = RemoteDriverAtataContextBuilderOverride.Context;

            using (context.UseNullDriver())
            {
                AtataContextBuilder builder = AtataContext.Configure().
                    ApplyJsonConfig(@"Configs/Remote.json");

                builder.BuildingContext.DriverFactoryToUse.Create();
            }

            context.RemoteAddress.Should().Be("http://127.0.0.1:8888/wd/hub");
            context.CommandTimeout.Should().Be(TimeSpan.FromSeconds(100));
        }

        [Test]
        public void Driver_Remote_WithOptions()
        {
            var context = RemoteDriverAtataContextBuilderOverride.Context;

            using (context.UseNullDriver())
            {
                AtataContextBuilder builder = AtataContext.Configure().
                    ApplyJsonConfig(@"Configs/RemoteChrome.json");

                builder.BuildingContext.DriverFactoryToUse.Create();
            }

            ICapabilities capabilities = context.Capabilities;

            capabilities.GetCapability(CapabilityType.BrowserName).Should().Be(DriverAliases.Chrome);

            var chromeCapabilities = (Dictionary<string, object>)capabilities.GetCapability(ChromeOptions.Capability);

            chromeCapabilities.Should().Equal(new Dictionary<string, object>
            {
                ["detach"] = true,
                ["cap1"] = true,
                ["cap2"] = 5,
                ["cap3"] = "str"
            });
        }

        [Test]
        public void Driver_Remote_WithTypelessOptions()
        {
            var context = RemoteDriverAtataContextBuilderOverride.Context;

            using (context.UseNullDriver())
            {
                AtataContextBuilder builder = AtataContext.Configure().
                    ApplyJsonConfig(@"Configs/RemoteTypeless.json");

                Assert.Throws<ArgumentNullException>(() =>
                    builder.BuildingContext.DriverFactoryToUse.Create());
            }
        }

        [Test]
        public void Driver_Remote_WithoutType()
        {
            var context = RemoteDriverAtataContextBuilderOverride.Context;

            using (context.UseNullDriver())
            {
                AtataContextBuilder builder = AtataContext.Configure().
                    ApplyJsonConfig(@"Configs/RemoteFirefox.json");

                builder.BuildingContext.DriverFactoryToUse.Create();
            }

            ICapabilities capabilities = context.Capabilities;

            capabilities.GetCapability(CapabilityType.BrowserName).Should().Be(DriverAliases.Firefox);
        }

        [Test]
        public void Driver_Multiple()
        {
            AtataContextBuilder builder = AtataContext.Configure().
                ApplyJsonConfig(@"Configs/MultipleDrivers.json");

            builder.BuildingContext.DriverFactories.Should().HaveCount(2);
            builder.BuildingContext.DriverFactories[0].Alias.Should().Be(DriverAliases.Chrome);
            builder.BuildingContext.DriverFactories[1].Alias.Should().Be(DriverAliases.Firefox);
            builder.BuildingContext.DriverFactoryToUse.Alias.Should().Be(DriverAliases.Firefox);
        }

        [Test]
        public void Driver_Multiple_ThruGlobalConfiguration()
        {
            AtataContext.GlobalConfiguration.
                ApplyJsonConfig(@"Configs/MultipleDrivers.json");

            var driver = JsonConfig.Global.Driver;

            AtataContextBuilder builder = AtataContext.Configure();

            builder.BuildingContext.DriverFactories.Should().HaveCount(2);
            builder.BuildingContext.DriverFactories[0].Alias.Should().Be(DriverAliases.Chrome);
            builder.BuildingContext.DriverFactories[1].Alias.Should().Be(DriverAliases.Firefox);
            builder.BuildingContext.DriverFactoryToUse.Alias.Should().Be(DriverAliases.Firefox);

            builder.Build();
            AtataContext.Current.Driver.Should().BeOfType<FirefoxDriver>();
            JsonConfig.Global.Drivers.Should().HaveCount(2);
            JsonConfig.Current.Drivers.Should().HaveCount(2);
        }
    }
}
