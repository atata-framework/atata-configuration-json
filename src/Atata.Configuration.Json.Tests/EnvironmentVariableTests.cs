using System;
using FluentAssertions;
using NUnit.Framework;

namespace Atata.Configuration.Json.Tests
{
    public class EnvironmentVariableTests : TestFixture
    {
        private const string VariableName = "baseurl";

        [SetUp]
        public void SetUp()
        {
            Environment.SetEnvironmentVariable(VariableName, null);
        }

        [Test]
        public void Found()
        {
            string variableValue = "https://example.org/";
            Environment.SetEnvironmentVariable(VariableName, variableValue, EnvironmentVariableTarget.Process);

            AtataContextBuilder builder = AtataContext.Configure()
                .ApplyJsonConfig("Configs/EnvironmentVariables");

            builder.BuildingContext.BaseUrl.Should().Be(variableValue);
        }

        [Test]
        public void NotFound()
        {
            AtataContextBuilder builder = AtataContext.Configure();

            var exception = Assert.Throws<ConfigurationException>(() =>
                builder.ApplyJsonConfig("Configs/EnvironmentVariables"));

            exception.Message.Should().Contain(VariableName);
        }
    }
}
