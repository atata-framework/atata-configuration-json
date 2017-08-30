using System;

namespace Atata
{
    public static class JsonConfigMapper
    {
        public static AtataContextBuilder Map<TConfig>(TConfig config, AtataContextBuilder builder)
            where TConfig : JsonConfig<TConfig>
        {
            if (config.BaseUrl != null)
                builder.UseBaseUrl(config.BaseUrl);

            if (config.RetryTimeout != null)
                builder.UseRetryTimeout(TimeSpan.FromSeconds(config.RetryTimeout.Value));

            if (config.RetryInterval != null)
                builder.UseRetryInterval(TimeSpan.FromSeconds(config.RetryInterval.Value));

            if (config.AssertionExceptionType != null)
                builder.UseAssertionExceptionType(Type.GetType(config.AssertionExceptionType, true));

            if (config.UseNUnitTestName)
                builder.UseNUnitTestName();

            if (config.LogNUnitError)
                builder.LogNUnitError();

            if (config.TakeScreenshotOnNUnitError)
            {
                if (config.TakeScreenshotOnNUnitErrorTitle != null)
                    builder.TakeScreenshotOnNUnitError(config.TakeScreenshotOnNUnitErrorTitle);
                else
                    builder.TakeScreenshotOnNUnitError();
            }

            if (config.LogConsumers != null)
            {
                foreach (var item in config.LogConsumers)
                    MapLogConsumer(item, builder);
            }

            if (config.ScreenshotConsumers != null)
            {
                foreach (var item in config.ScreenshotConsumers)
                    MapScreenshotConsumer(item, builder);
            }

            return builder;
        }

        private static void MapLogConsumer(LogConsumerJsonSection consumerSection, AtataContextBuilder builder)
        {
            var consumerBuilder = builder.AddLogConsumer(consumerSection.Type);

            if (consumerSection.MinLevel != null)
                consumerBuilder.WithMinLevel(consumerSection.MinLevel.Value);

            if (consumerSection.SectionFinish == false)
                consumerBuilder.WithoutSectionFinish();

            consumerBuilder.WithProperties(consumerSection.ExtraPropertiesMap);
        }

        private static void MapScreenshotConsumer(ScreenshotConsumerJsonSection consumerSection, AtataContextBuilder builder)
        {
            var consumerBuilder = builder.AddScreenshotConsumer(consumerSection.Type);

            consumerBuilder.WithProperties(consumerSection.ExtraPropertiesMap);
        }
    }
}
