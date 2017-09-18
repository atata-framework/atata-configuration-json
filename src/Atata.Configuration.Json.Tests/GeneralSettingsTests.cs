using System;
using FluentAssertions;
using NUnit.Framework;

namespace Atata.Configuration.Json.Tests
{
    [TestFixture]
    public class GeneralSettingsTests
    {
        [Test]
        public void GeneralSettings_NUnit()
        {
            AtataContextBuilder builder = AtataContext.Configure().
                ApplyJsonConfig("Configs/Chrome+NUnit.json");

            var context = builder.BuildingContext;

            context.BaseUrl.Should().Be("https://atata-framework.github.io/atata-sample-app/#!/");

            context.CleanUpActions.Should().HaveCount(2);

            context.AssertionExceptionType.Should().Be(typeof(NUnit.Framework.AssertionException));

            context.RetryTimeout.Should().Be(TimeSpan.FromSeconds(7));
            context.RetryInterval.Should().Be(TimeSpan.FromSeconds(0.7));

            context.TestNameFactory().Should().Be(nameof(GeneralSettings_NUnit));
        }
    }
}
