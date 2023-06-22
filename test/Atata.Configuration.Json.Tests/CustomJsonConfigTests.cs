using Newtonsoft.Json;

namespace Atata.Configuration.Json.Tests;

[TestFixture]
public class CustomJsonConfigTests : TestFixture
{
    [Test]
    public void Default()
    {
        string jsonContent = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Configs/CustomSettings.json"));
        JsonConfig config = JsonConvert.DeserializeObject<JsonConfig>(jsonContent);

        AtataContextBuilder builder = AtataContext.Configure()
            .ApplyJsonConfig(config);

        JsonConfig.Current.Should().BeNull();

        config.BaseUrl.Should().Be("https://demo.atata.io/");

        builder.BuildingContext.DriverFactories.Should().HaveCount(1);
        builder.BuildingContext.BaseUrl.Should().Be("https://demo.atata.io/");
    }

    [Test]
    public void Custom()
    {
        string jsonContent = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Configs/CustomSettings.json"));
        CustomJsonConfig config = JsonConvert.DeserializeObject<CustomJsonConfig>(jsonContent);

        AtataContextBuilder builder = AtataContext.Configure()
            .ApplyJsonConfig(config);

        CustomJsonConfig.Current.Should().BeNull();

        config.BaseUrl.Should().Be("https://demo.atata.io/");
        config.IntProperty.Should().Be(5);
        config.StringArrayValues.Should().Equal("str1", "str2", "str3");

        builder.BuildingContext.DriverFactories.Should().HaveCount(1);
        builder.BuildingContext.BaseUrl.Should().Be("https://demo.atata.io/");
    }
}
