using System;
using System.IO;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Atata.Configuration.Json.Tests
{
    [TestFixture]
    public class CustomJsonConfigTests : TestFixture
    {
        [Test]
        public void CustomJsonConfig_Default()
        {
            string jsonContent = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Configs/CustomSettings.json"));
            JsonConfig config = JsonConvert.DeserializeObject<JsonConfig>(jsonContent);

            AtataContextBuilder builder = AtataContext.Configure().
                ApplyJsonConfig(config);

            JsonConfig.Current.Should().BeNull();

            config.BaseUrl.Should().Be("https://atata-framework.github.io/atata-sample-app/#!/");

            builder.BuildingContext.DriverFactories.Should().HaveCount(1);
            builder.BuildingContext.BaseUrl.Should().Be("https://atata-framework.github.io/atata-sample-app/#!/");
        }

        [Test]
        public void CustomJsonConfig_Custom()
        {
            string jsonContent = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Configs/CustomSettings.json"));
            CustomJsonConfig config = JsonConvert.DeserializeObject<CustomJsonConfig>(jsonContent);

            AtataContextBuilder builder = AtataContext.Configure().
                ApplyJsonConfig(config);

            CustomJsonConfig.Current.Should().BeNull();

            config.BaseUrl.Should().Be("https://atata-framework.github.io/atata-sample-app/#!/");
            config.IntProperty.Should().Be(5);
            config.StringArrayValues.Should().Equal(new[] { "str1", "str2", "str3" });

            builder.BuildingContext.DriverFactories.Should().HaveCount(1);
            builder.BuildingContext.BaseUrl.Should().Be("https://atata-framework.github.io/atata-sample-app/#!/");
        }
    }
}
