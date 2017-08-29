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

            if (config.AssertionExceptionTypeName != null)
                builder.UseAssertionExceptionType(Type.GetType(config.AssertionExceptionTypeName, true));

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

            return builder;
        }

        private static void MapLogConsumer(LogConsumerJsonSection logConsumerSection, AtataContextBuilder builder)
        {
            var consumerBuilder = builder.AddLogConsumer(logConsumerSection.TypeName);

            if (logConsumerSection.MinLevel != null)
                consumerBuilder.WithMinLevel(logConsumerSection.MinLevel.Value);

            if (logConsumerSection.SectionFinish == false)
                consumerBuilder.WithoutSectionFinish();

            consumerBuilder.WithProperties(logConsumerSection.ExtraPropertiesMap);
        }
    }
}
