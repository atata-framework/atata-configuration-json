using System;
using System.IO;
using FluentAssertions;
using NUnit.Framework;

namespace Atata.Configuration.Json.Tests
{
    [TestFixture]
    public class ConfigFilePathTests
    {
        [Test]
        public void ConfigFilePath_Default()
        {
            AtataContextBuilder builder = AtataContext.Build().
                ApplyJsonConfig();

            builder.BuildingContext.BaseUrl.Should().EndWith("default");
        }

        [Test]
        public void ConfigFilePath_FullPath()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Simple.json");

            AtataContextBuilder builder = AtataContext.Build().
                ApplyJsonConfig(filePath);

            builder.BuildingContext.BaseUrl.Should().EndWith("simple");
        }

        [Test]
        public void ConfigFilePath_FullPathWthAlias()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Simple.json");

            AtataContextBuilder builder = AtataContext.Build().
                ApplyJsonConfig(filePath, "QA");

            builder.BuildingContext.BaseUrl.Should().EndWith("simple.qa");
        }
    }
}
