using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Atata.Configuration.Json
{
    public static class JsonConfigMapper
    {
        public static AtataContextBuilder Map<TConfig>(TConfig config, AtataContextBuilder builder)
            where TConfig : JsonConfig<TConfig>
        {
            if (config.DriverInitializationStage != null)
                builder.UseDriverInitializationStage(config.DriverInitializationStage.Value);

            if (config.BaseUrl != null)
                builder.UseBaseUrl(config.BaseUrl);

            if (config.Culture != null)
                builder.UseCulture(config.Culture);

            if (config.TimeZone != null)
                builder.UseTimeZone(config.TimeZone);

            if (config.ArtifactsPath != null)
                builder.UseArtifactsPath(config.ArtifactsPath);

            if (config.BaseRetryTimeout != null)
                builder.UseBaseRetryTimeout(TimeSpan.FromSeconds(config.BaseRetryTimeout.Value));

            if (config.BaseRetryInterval != null)
                builder.UseBaseRetryInterval(TimeSpan.FromSeconds(config.BaseRetryInterval.Value));

            if (config.ElementFindTimeout != null)
                builder.UseElementFindTimeout(TimeSpan.FromSeconds(config.ElementFindTimeout.Value));

            if (config.ElementFindRetryInterval != null)
                builder.UseElementFindRetryInterval(TimeSpan.FromSeconds(config.ElementFindRetryInterval.Value));

            if (config.WaitingTimeout != null)
                builder.UseWaitingTimeout(TimeSpan.FromSeconds(config.WaitingTimeout.Value));

            if (config.WaitingRetryInterval != null)
                builder.UseWaitingRetryInterval(TimeSpan.FromSeconds(config.WaitingRetryInterval.Value));

            if (config.VerificationTimeout != null)
                builder.UseVerificationTimeout(TimeSpan.FromSeconds(config.VerificationTimeout.Value));

            if (config.VerificationRetryInterval != null)
                builder.UseVerificationRetryInterval(TimeSpan.FromSeconds(config.VerificationRetryInterval.Value));

            if (config.DefaultAssemblyNamePatternToFindTypes != null)
                builder.UseDefaultAssemblyNamePatternToFindTypes(config.DefaultAssemblyNamePatternToFindTypes);

            if (config.AssemblyNamePatternToFindComponentTypes != null)
                builder.UseAssemblyNamePatternToFindComponentTypes(config.AssemblyNamePatternToFindComponentTypes);

            if (config.AssemblyNamePatternToFindAttributeTypes != null)
                builder.UseAssemblyNamePatternToFindAttributeTypes(config.AssemblyNamePatternToFindAttributeTypes);

            Lazy<Assembly[]> lazyAssembliesToFindTypesIn = new Lazy<Assembly[]>(
                () => AssemblyFinder.FindAllByPattern(builder.BuildingContext.DefaultAssemblyNamePatternToFindTypes),
                isThreadSafe: false);

            if (config.AssertionExceptionType != null)
                builder.UseAssertionExceptionType(
                    TypeFinder.FindInAssemblies(config.AssertionExceptionType, lazyAssembliesToFindTypesIn.Value));

            if (config.AggregateAssertionExceptionType != null)
                builder.UseAggregateAssertionExceptionType(
                    TypeFinder.FindInAssemblies(config.AggregateAssertionExceptionType, lazyAssembliesToFindTypesIn.Value));

            if (config.AggregateAssertionStrategyType != null)
                builder.UseAggregateAssertionStrategy(
                    ActivatorEx.CreateInstance<IAggregateAssertionStrategy>(
                        TypeFinder.FindInAssemblies(config.AggregateAssertionStrategyType, lazyAssembliesToFindTypesIn.Value)));

            if (config.WarningReportStrategyType != null)
                builder.UseWarningReportStrategy(
                    ActivatorEx.CreateInstance<IWarningReportStrategy>(
                        TypeFinder.FindInAssemblies(config.WarningReportStrategyType, lazyAssembliesToFindTypesIn.Value)));

            if (config.UseNUnitTestName)
                builder.UseNUnitTestName();

            if (config.UseNUnitTestSuiteName)
                builder.UseNUnitTestSuiteName();

            if (config.UseNUnitTestSuiteType)
                builder.UseNUnitTestSuiteType();

            if (config.LogNUnitError)
                builder.LogNUnitError();

            if (config.TakeScreenshotOnNUnitError)
            {
                if (config.TakeScreenshotOnNUnitErrorTitle != null)
                    builder.TakeScreenshotOnNUnitError(config.TakeScreenshotOnNUnitErrorTitle);
                else
                    builder.TakeScreenshotOnNUnitError();
            }

            if (config.OnCleanUpAddArtifactsToNUnitTestContext)
                builder.OnCleanUpAddArtifactsToNUnitTestContext();

            if (config.OnCleanUpAddDirectoryFilesToNUnitTestContext != null)
                builder.OnCleanUpAddDirectoryFilesToNUnitTestContext(config.OnCleanUpAddDirectoryFilesToNUnitTestContext);

            if (config.UseNUnitAggregateAssertionStrategy)
                builder.UseNUnitAggregateAssertionStrategy();

            if (config.UseNUnitWarningReportStrategy)
                builder.UseNUnitWarningReportStrategy();

            if (config.UseAllNUnitFeatures)
                builder.UseAllNUnitFeatures();

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

            if (config.Drivers != null)
            {
                foreach (var item in config.Drivers)
                    MapDriver(item, builder);
            }

            if (config.Attributes != null)
                MapAttributes(config.Attributes, builder);

            return builder;
        }

        private static void MapLogConsumer(LogConsumerJsonSection section, AtataContextBuilder builder)
        {
            var consumerBuilder = builder.AddLogConsumer(section.Type);

            if (section.MinLevel != null)
                consumerBuilder.WithMinLevel(section.MinLevel.Value);

            if (section.SectionFinish == false)
                consumerBuilder.WithoutSectionFinish();

            if (section.MessageNestingLevelIndent != null)
                consumerBuilder.WithMessageNestingLevelIndent(section.MessageNestingLevelIndent);

            if (section.MessageStartSectionPrefix != null)
                consumerBuilder.WithMessageStartSectionPrefix(section.MessageStartSectionPrefix);

            if (section.MessageEndSectionPrefix != null)
                consumerBuilder.WithMessageEndSectionPrefix(section.MessageEndSectionPrefix);

            if (consumerBuilder.Context is NLogFileConsumer nLogFileConsumer)
                ConfigureNLogFileConsumer(nLogFileConsumer, section.ExtraPropertiesMap);
            else
                consumerBuilder.WithProperties(section.ExtraPropertiesMap);
        }

        // TODO: Remove this method when NLogFileConsumer will get string path/name properties.
        private static void ConfigureNLogFileConsumer(NLogFileConsumer consumer, Dictionary<string, object> propertiesMap)
        {
            foreach (var item in propertiesMap)
            {
                if (item.Key.Equals("FolderPath", StringComparison.OrdinalIgnoreCase))
                    consumer.FolderPathBuilder = _ => item.Value.ToString();
                else if (item.Key.Equals("FileName", StringComparison.OrdinalIgnoreCase))
                    consumer.FileNameBuilder = _ => item.Value.ToString();
                else if (item.Key.Equals("FilePath", StringComparison.OrdinalIgnoreCase))
                    consumer.FilePathBuilder = _ => item.Value.ToString();
            }
        }

        private static void MapScreenshotConsumer(ScreenshotConsumerJsonSection section, AtataContextBuilder builder)
        {
            var consumerBuilder = builder.AddScreenshotConsumer(section.Type);

            consumerBuilder.WithProperties(section.ExtraPropertiesMap);
        }

        private static void MapDriver(DriverJsonSection section, AtataContextBuilder builder)
        {
            IDriverJsonMapper mapper = DriverJsonMapperAliases.Resolve(section.Type);
            mapper.Map(section, builder);
        }

        private static void MapAttributes(AttributesJsonSection attributesSection, AtataContextBuilder builder)
        {
            AttributeMapper attributeMapper = new AttributeMapper(
                builder.BuildingContext.AssemblyNamePatternToFindAttributeTypes ?? builder.BuildingContext.DefaultAssemblyNamePatternToFindTypes,
                builder.BuildingContext.DefaultAssemblyNamePatternToFindTypes);

            if (attributesSection.Global != null)
            {
                builder.Attributes.Global.Add(
                    attributesSection.Global.Select(attributeMapper.Map));
            }

            if (attributesSection.Assembly != null)
            {
                foreach (AssemblyAttributesJsonSection assemblySection in attributesSection.Assembly)
                {
                    if (string.IsNullOrEmpty(assemblySection.Name))
                        throw new ConfigurationException(
                            "\"name\" configuration property of assembly section is not specified.");

                    if (assemblySection.Attributes != null)
                        builder.Attributes.Assembly(assemblySection.Name).Add(
                            assemblySection.Attributes.Select(attributeMapper.Map));
                }
            }

            if (attributesSection.Component != null)
            {
                foreach (ComponentAttributesJsonSection componentSection in attributesSection.Component)
                {
                    if (string.IsNullOrEmpty(componentSection.Type))
                        throw new ConfigurationException(
                            "\"type\" configuration property of component section is not specified.");

                    var componentAttributesBuilder = builder.Attributes.Component(componentSection.Type);

                    if (componentSection.Attributes != null)
                        componentAttributesBuilder.Add(componentSection.Attributes.Select(attributeMapper.Map));

                    if (componentSection.Properties != null)
                    {
                        foreach (PropertyAttributesJsonSection propertySection in componentSection.Properties)
                        {
                            if (string.IsNullOrEmpty(propertySection.Name))
                                throw new ConfigurationException(
                                    "\"name\" configuration property of property section is not specified.");

                            if (propertySection.Attributes != null)
                                componentAttributesBuilder.Property(propertySection.Name).Add(
                                    propertySection.Attributes.Select(attributeMapper.Map));
                        }
                    }
                }
            }
        }
    }
}
