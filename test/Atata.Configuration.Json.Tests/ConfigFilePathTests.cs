using System;
using System.IO;
using FluentAssertions;
using NUnit.Framework;

namespace Atata.Configuration.Json.Tests
{
    [TestFixture]
    public class ConfigFilePathTests : TestFixture
    {
        [Test]
        public void Default()
        {
            AtataContextBuilder builder = AtataContext.Configure().
                ApplyJsonConfig();

            builder.BuildingContext.BaseUrl.Should().EndWith("atata");
        }

        [Test]
        public void Default_WithAlias()
        {
            AtataContextBuilder builder = AtataContext.Configure().
                ApplyJsonConfig(environmentAlias: "QA");

            builder.BuildingContext.BaseUrl.Should().EndWith("atata.qa");
        }

        [Test]
        public void FileName()
        {
            string filePath = "Simple.json";

            AtataContextBuilder builder = AtataContext.Configure().
                ApplyJsonConfig(filePath);

            builder.BuildingContext.BaseUrl.Should().EndWith("simple");
        }

        [Test]
        public void FileName_WithAlias()
        {
            string filePath = "Simple.json";

            AtataContextBuilder builder = AtataContext.Configure().
                ApplyJsonConfig(filePath, "QA");

            builder.BuildingContext.BaseUrl.Should().EndWith("simple.qa");
        }

        [Test]
        public void FileName_WithoutExtension()
        {
            string filePath = "Simple";

            AtataContextBuilder builder = AtataContext.Configure().
                ApplyJsonConfig(filePath);

            builder.BuildingContext.BaseUrl.Should().EndWith("simple");
        }

        [Test]
        public void FileName_WithAlias_WithoutExtension()
        {
            string filePath = "Simple";

            AtataContextBuilder builder = AtataContext.Configure().
                ApplyJsonConfig(filePath, "QA");

            builder.BuildingContext.BaseUrl.Should().EndWith("simple.qa");
        }

        [Test]
        public void AbsolutePath()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Simple.json");

            AtataContextBuilder builder = AtataContext.Configure().
                ApplyJsonConfig(filePath);

            builder.BuildingContext.BaseUrl.Should().EndWith("simple");
        }

        [Test]
        public void AbsolutePath_WithAlias()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Simple.json");

            AtataContextBuilder builder = AtataContext.Configure().
                ApplyJsonConfig(filePath, "QA");

            builder.BuildingContext.BaseUrl.Should().EndWith("simple.qa");
        }

        [Test]
        public void DirectoryPath()
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory;

            AtataContextBuilder builder = AtataContext.Configure().
                ApplyJsonConfig(filePath);

            builder.BuildingContext.BaseUrl.Should().EndWith("atata");
        }

        [Test]
        public void DirectoryPath_WithAlias()
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory;

            AtataContextBuilder builder = AtataContext.Configure().
                ApplyJsonConfig(filePath, "QA");

            builder.BuildingContext.BaseUrl.Should().EndWith("atata.qa");
        }
    }
}
