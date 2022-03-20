using System;
using System.IO;
using NUnit.Framework;

namespace Atata.Configuration.Json.Tests
{
    [TestFixture]
    public class StandardSettingsTests : TestFixture
    {
        [Test]
        public void Regular()
        {
            Subject<AtataContext> result = AtataContext.Configure()
                .ApplyJsonConfig(@"Configs/StandardSettings.json")
                .Build()
                .ToResultSubject();

            result.ValueOf(x => x.DriverInitializationStage).Should.Equal(AtataContextDriverInitializationStage.OnDemand);

            result.ValueOf(x => x.BaseUrl).Should.Equal("https://demo.atata.io/");

            result.ValueOf(x => x.Artifacts.FullName.Value).Should.Equal(
                Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "artifacts",
                    DateTime.Now.Year.ToString(),
                    nameof(StandardSettingsTests),
                    TestContext.CurrentContext.Test.Name));

            result.ValueOf(x => x.TimeZone.Id).Should.Equal("UTC");

            result.ValueOf(x => x.Variables["customIntVar"]).Should.Be(7L);
            result.ValueOf(x => x.Variables["customStringVar"]).Should.Be("strvar");
        }
    }
}
