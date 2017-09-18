using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;

namespace Atata.Configuration.Json.Tests
{
    [TestFixture]
    public class ScreenshotConsumerTests
    {
        [Test]
        public void ScreenshotConsumer_AllKinds()
        {
            AtataContextBuilder builder = AtataContext.Configure().
                ApplyJsonConfig("ScreenshotConsumers");

            IScreenshotConsumer[] expected =
            {
                new FileScreenshotConsumer { ImageFormat = ScreenshotImageFormat.Png, FilePath = "/logs/{test-name}.txt" },
                new CustomScreenshotConsumer { IntProperty = 15 },
                new FileScreenshotConsumer { ImageFormat = ScreenshotImageFormat.Jpeg, FolderPath = "/logs", FileName = "{test-name}" }
            };

            builder.BuildingContext.ScreenshotConsumers.Select(x => x.GetType()).Should().BeEquivalentTo(expected.Select(x => x.GetType()));

            builder.BuildingContext.ScreenshotConsumers.ShouldAllBeEquivalentTo(
                expected,
                opt => opt.IncludingAllRuntimeProperties());
        }

        public class CustomScreenshotConsumer : IScreenshotConsumer
        {
            public int? IntProperty { get; set; }

            public void Take(ScreenshotInfo screenshotInfo)
            {
                throw new NotImplementedException();
            }
        }
    }
}
