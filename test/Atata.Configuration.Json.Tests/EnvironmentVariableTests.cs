using System;
using FluentAssertions;
using NUnit.Framework;

namespace Atata.Configuration.Json.Tests
{
    public class EnvironmentVariableTests : TestFixture
    {
        private const string Variable1Name = "url_prefix";

        private const string Variable2Name = "url_suffix";

        [SetUp]
        public void SetUp()
        {
            Environment.SetEnvironmentVariable(Variable1Name, null);
        }

        [Test]
        public void Found()
        {
            Environment.SetEnvironmentVariable(Variable1Name, "https://example.org", EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable(Variable2Name, "test", EnvironmentVariableTarget.Process);

            AtataContextBuilder builder = AtataContext.Configure()
                .ApplyJsonConfig("Configs/EnvironmentVariables");

            builder.BuildingContext.BaseUrl.Should().Be("https://example.org/test");
        }

        [Test]
        public void NotFound()
        {
            AtataContextBuilder builder = AtataContext.Configure();

            var exception = Assert.Throws<ConfigurationException>(() =>
                builder.ApplyJsonConfig("Configs/EnvironmentVariables"));

            exception.Message.Should().Contain(Variable1Name);
        }
    }
}
