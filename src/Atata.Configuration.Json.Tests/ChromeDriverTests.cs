using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace Atata.Configuration.Json.Tests
{
    [TestFixture]
    public class ChromeDriverTests
    {
        [Test]
        public void Driver_Chrome()
        {
            var context = ChromeAtataContextBuilderOverride.Context;

            using (context.UseNullDriver())
            {
                AtataContextBuilder builder = AtataContext.Build().
                    ApplyJsonConfig(@"Configs\Chrome.json");

                builder.BuildingContext.DriverCreator();
            }

            var capabilities = context.Options.ToCapabilities();

            capabilities.GetCapability(CapabilityType.LoggingPreferences).ShouldBeEquivalentTo(new Dictionary<string, object>
            {
                ["browser"] = "INFO",
                ["driver"] = "WARNING"
            });

            var chromeCapabilities = (Dictionary<string, object>)capabilities.GetCapability(ChromeOptions.Capability);

            chromeCapabilities.Should().Contain(new Dictionary<string, object>
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

            ((List<string>)chromeCapabilities["excludeSwitches"]).Should().Equal("exc-arg");

            context.Options.Extensions.Should().Equal("ZW5jLWV4dDE=", "ZW5jLWV4dDI=");

            ((List<string>)chromeCapabilities["windowTypes"]).Should().Equal("win1", "win2");

            context.Options.PerformanceLoggingPreferences.IsCollectingPageEvents.Should().BeFalse();
            context.Options.PerformanceLoggingPreferences.IsCollectingTimelineEvents.Should().BeFalse();
            context.Options.PerformanceLoggingPreferences.BufferUsageReportingInterval.Should().Be(TimeSpan.FromSeconds(70));
            context.Options.PerformanceLoggingPreferences.TracingCategories.Should().Be("cat1,cat2");

            ((Dictionary<string, object>)chromeCapabilities["prefs"]).Should().Equal(new Dictionary<string, object>
            {
                ["pref1"] = 7,
                ["pref2"] = false,
                ["pref3"] = "str"
            });

            ((Dictionary<string, object>)chromeCapabilities["localState"]).Should().Equal(new Dictionary<string, object>
            {
                ["pref1"] = 2.7,
                ["pref2"] = true,
                ["pref3"] = string.Empty
            });

            ((Dictionary<string, object>)chromeCapabilities["mobileEmulation"]).Should().Equal(new Dictionary<string, object>
            {
                ["deviceName"] = "emul"
            });

            context.Options.LeaveBrowserRunning.Should().BeTrue();
            context.Options.MinidumpPath.Should().Be("mdp");
        }
    }
}
