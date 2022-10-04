using OpenQA.Selenium;

namespace Atata.Configuration.Json.Tests;

[TestFixture]
public class ScreenshotConsumerTests : TestFixture
{
    [Test]
    public void Multiple_ViaSingleConfig()
    {
        AtataContextBuilder builder = AtataContext.Configure()
            .ApplyJsonConfig("ScreenshotConsumers");

        IScreenshotConsumer[] expected =
        {
            new FileScreenshotConsumer { ImageFormat = ScreenshotImageFormat.Png, FilePath = "/logs/{test-name}.txt" },
            new CustomScreenshotConsumer { IntProperty = 15 },
            new FileScreenshotConsumer { ImageFormat = ScreenshotImageFormat.Jpeg, DirectoryPath = "/logs", FileName = "{test-name}" }
        };

        builder.BuildingContext.ScreenshotConsumers.Select(x => x.GetType()).Should().BeEquivalentTo(expected.Select(x => x.GetType()));

        builder.BuildingContext.ScreenshotConsumers.Should().BeEquivalentTo(
            expected,
            opt => opt.IncludingAllRuntimeProperties());
    }

    public class CustomScreenshotConsumer : IScreenshotConsumer
    {
        public int? IntProperty { get; set; }

        public void Take(ScreenshotInfo screenshotInfo) =>
            throw new NotSupportedException();
    }
}
