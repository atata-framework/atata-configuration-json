using System;
using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;

namespace Atata.Configuration.Json.Tests
{
    [TestFixture]
    public class GeneralSettingsTests : TestFixture
    {
        [Test]
        public void GeneralAndNUnit()
        {
            AtataContextBuilder builder = AtataContext.Configure().
                ApplyJsonConfig("Configs/Chrome+NUnit.json");

            var context = builder.BuildingContext;

            using (new AssertionScope())
            {
                context.BaseUrl.Should().Be("https://demo.atata.io/");

                context.Culture.Name.Should().Be("en-US");

                context.CleanUpActions.Should().HaveCount(3);

                context.AssertionExceptionType.Should().Be(typeof(NUnit.Framework.AssertionException));

                context.BaseRetryTimeout.Should().Be(TimeSpan.FromSeconds(7));
                context.BaseRetryInterval.Should().Be(TimeSpan.FromSeconds(0.7));

                context.ElementFindTimeout.Should().Be(TimeSpan.FromSeconds(8));
                context.ElementFindRetryInterval.Should().Be(TimeSpan.FromSeconds(0.8));

                context.WaitingTimeout.Should().Be(TimeSpan.FromSeconds(9));
                context.WaitingRetryInterval.Should().Be(TimeSpan.FromSeconds(0.9));

                context.VerificationTimeout.Should().Be(TimeSpan.FromSeconds(10));
                context.VerificationRetryInterval.Should().Be(TimeSpan.FromSeconds(1));

                context.TestNameFactory().Should().Be(nameof(GeneralAndNUnit));

                context.DefaultAssemblyNamePatternToFindTypes.Should().Be("def");
                context.AssemblyNamePatternToFindComponentTypes.Should().Be("comp");
                context.AssemblyNamePatternToFindAttributeTypes.Should().Be("attr");
            }
        }
    }
}
