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
        public void LogConsumer_AllKinds()
        {
            AtataContextBuilder builder = AtataContext.Build().
                ApplyJsonConfig("ScreenshotConsumers");

            IScreenshotConsumer[] expected =
            {
                new FileScreenshotConsumer { ImageFormat = ScreenshotImageFormat.Png },
                new CustomScreenshotConsumer { IntProperty = 15 },
                new FileScreenshotConsumer { ImageFormat = ScreenshotImageFormat.Jpeg },
            };

            builder.BuildingContext.ScreenshotConsumers.Should().BeEquivalentTo(expected.Select(x => x.GetType()));

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
