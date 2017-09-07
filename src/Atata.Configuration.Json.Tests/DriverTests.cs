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
    public class DriverTests
    {
        [Test]
        public void Driver_Chrome()
        {
            var context = ChromeAtataContextBuilderOverride.Context;

            using (context.UseNullDriver())
            {
                AtataContextBuilder builder = AtataContext.Build().
                    ApplyJsonConfig(@"Configs/Chrome.json");

                builder.BuildingContext.DriverCreator();
            }

            var capabilities = context.Options.ToCapabilities();
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

            context.Options.Proxy.Kind.Should().Be(ProxyKind.Manual);
            context.Options.Proxy.HttpProxy.Should().Be("http");
            context.Options.Proxy.FtpProxy.Should().Be("ftp");

            context.Options.Arguments.Should().Equal("disable-extensions", "no-sandbox", "start-maximized");

            ((List<string>)optionsCapabilities["excludeSwitches"]).Should().Equal("exc-arg");

            context.Options.Extensions.Should().Equal("ZW5jLWV4dDE=", "ZW5jLWV4dDI=");

            ((List<string>)optionsCapabilities["windowTypes"]).Should().Equal("win1", "win2");

            context.Options.PerformanceLoggingPreferences.IsCollectingPageEvents.Should().BeFalse();
            context.Options.PerformanceLoggingPreferences.IsCollectingTimelineEvents.Should().BeFalse();
            context.Options.PerformanceLoggingPreferences.BufferUsageReportingInterval.Should().Be(TimeSpan.FromSeconds(70));
            context.Options.PerformanceLoggingPreferences.TracingCategories.Should().Be("cat1,cat2");

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

            context.Options.LeaveBrowserRunning.Should().BeTrue();
            context.Options.MinidumpPath.Should().Be("mdp");

            context.Service.Port.Should().Be(555);
            context.Service.WhitelistedIPAddresses.Should().Be("5.5.5.5,7.7.7.7");

            context.CommandTimeout.Should().Be(TimeSpan.FromMinutes(1));
        }

        [Test]
        public void Driver_Firefox()
        {
            var context = FirefoxAtataContextBuilderOverride.Context;

            using (context.UseNullDriver())
            {
                AtataContextBuilder builder = AtataContext.Build().
                    ApplyJsonConfig(@"Configs/Firefox.json");

                builder.BuildingContext.DriverCreator();
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
                AtataContextBuilder builder = AtataContext.Build().
                    ApplyJsonConfig(@"Configs/InternetExplorer.json");

                builder.BuildingContext.DriverCreator();
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
            context.Options.UnexpectedAlertBehavior.Should().Be(InternetExplorerUnexpectedAlertBehavior.Dismiss);

            context.Service.LoggingLevel.Should().Be(InternetExplorerDriverLogLevel.Debug);

            context.CommandTimeout.Should().Be(TimeSpan.FromSeconds(45));
        }
    }
}
