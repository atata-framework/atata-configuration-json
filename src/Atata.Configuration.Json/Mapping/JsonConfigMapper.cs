namespace Atata.Configuration.Json;

public static class JsonConfigMapper
{
    public static AtataContextBuilder Map<TConfig>(TConfig config, AtataContextBuilder builder)
        where TConfig : JsonConfig<TConfig>
    {
        if (config.DriverInitializationStage is not null)
            builder.UseDriverInitializationStage(config.DriverInitializationStage.Value);

        if (config.BaseUrl is not null)
            builder.UseBaseUrl(config.BaseUrl);

        if (config.DefaultControlVisibility is not null)
            builder.UseDefaultControlVisibility(config.DefaultControlVisibility.Value);

        if (config.Culture is not null)
            builder.UseCulture(config.Culture);

        if (config.ArtifactsPathTemplate is not null)
            builder.UseArtifactsPathTemplate(config.ArtifactsPathTemplate);

        if (config.Variables is not null)
            builder.AddVariables(config.Variables);

        if (config.BaseRetryTimeout is not null)
            builder.UseBaseRetryTimeout(TimeSpan.FromSeconds(config.BaseRetryTimeout.Value));

        if (config.BaseRetryInterval is not null)
            builder.UseBaseRetryInterval(TimeSpan.FromSeconds(config.BaseRetryInterval.Value));

        if (config.ElementFindTimeout is not null)
            builder.UseElementFindTimeout(TimeSpan.FromSeconds(config.ElementFindTimeout.Value));

        if (config.ElementFindRetryInterval is not null)
            builder.UseElementFindRetryInterval(TimeSpan.FromSeconds(config.ElementFindRetryInterval.Value));

        if (config.WaitingTimeout is not null)
            builder.UseWaitingTimeout(TimeSpan.FromSeconds(config.WaitingTimeout.Value));

        if (config.WaitingRetryInterval is not null)
            builder.UseWaitingRetryInterval(TimeSpan.FromSeconds(config.WaitingRetryInterval.Value));

        if (config.VerificationTimeout is not null)
            builder.UseVerificationTimeout(TimeSpan.FromSeconds(config.VerificationTimeout.Value));

        if (config.VerificationRetryInterval is not null)
            builder.UseVerificationRetryInterval(TimeSpan.FromSeconds(config.VerificationRetryInterval.Value));

        if (config.DefaultAssemblyNamePatternToFindTypes is not null)
            builder.UseDefaultAssemblyNamePatternToFindTypes(config.DefaultAssemblyNamePatternToFindTypes);

        if (config.AssemblyNamePatternToFindComponentTypes is not null)
            builder.UseAssemblyNamePatternToFindComponentTypes(config.AssemblyNamePatternToFindComponentTypes);

        if (config.AssemblyNamePatternToFindAttributeTypes is not null)
            builder.UseAssemblyNamePatternToFindAttributeTypes(config.AssemblyNamePatternToFindAttributeTypes);

        if (config.AssemblyNamePatternToFindEventTypes is not null)
            builder.UseAssemblyNamePatternToFindEventTypes(config.AssemblyNamePatternToFindEventTypes);

        if (config.AssemblyNamePatternToFindEventHandlerTypes is not null)
            builder.UseAssemblyNamePatternToFindEventHandlerTypes(config.AssemblyNamePatternToFindEventHandlerTypes);

        Lazy<Assembly[]> lazyAssembliesToFindTypesIn = new Lazy<Assembly[]>(
            () => AssemblyFinder.FindAllByPattern(builder.BuildingContext.DefaultAssemblyNamePatternToFindTypes),
            isThreadSafe: false);

        if (config.AssertionExceptionType is not null)
            builder.UseAssertionExceptionType(
                TypeFinder.FindInAssemblies(config.AssertionExceptionType, lazyAssembliesToFindTypesIn.Value));

        if (config.AggregateAssertionExceptionType is not null)
            builder.UseAggregateAssertionExceptionType(
                TypeFinder.FindInAssemblies(config.AggregateAssertionExceptionType, lazyAssembliesToFindTypesIn.Value));

        if (config.AggregateAssertionStrategyType is not null)
            builder.UseAggregateAssertionStrategy(
                ActivatorEx.CreateInstance<IAggregateAssertionStrategy>(
                    TypeFinder.FindInAssemblies(config.AggregateAssertionStrategyType, lazyAssembliesToFindTypesIn.Value)));

        if (config.WarningReportStrategyType is not null)
            builder.UseWarningReportStrategy(
                ActivatorEx.CreateInstance<IWarningReportStrategy>(
                    TypeFinder.FindInAssemblies(config.WarningReportStrategyType, lazyAssembliesToFindTypesIn.Value)));

        if (config.AssertionFailureReportStrategyType is not null)
            builder.UseAssertionFailureReportStrategy(
                ActivatorEx.CreateInstance<IAssertionFailureReportStrategy>(
                    TypeFinder.FindInAssemblies(config.AssertionFailureReportStrategyType, lazyAssembliesToFindTypesIn.Value)));

        if (config.DomTestIdAttributeName is not null)
            builder.UseDomTestIdAttributeName(config.DomTestIdAttributeName);

        if (config.DomTestIdAttributeDefaultCase is not null)
            builder.UseDomTestIdAttributeDefaultCase(config.DomTestIdAttributeDefaultCase.Value);

        if (config.UseNUnitTestName)
            builder.UseNUnitTestName();

        if (config.UseNUnitTestSuiteName)
            builder.UseNUnitTestSuiteName();

        if (config.UseNUnitTestSuiteType)
            builder.UseNUnitTestSuiteType();

        List<string> warnings = [];

        if (config.UseNUnitAggregateAssertionStrategy)
            builder.UseNUnitAggregateAssertionStrategy();

        if (config.UseNUnitWarningReportStrategy)
            builder.UseNUnitWarningReportStrategy();

        if (config.UseNUnitAssertionFailureReportStrategy)
            builder.UseNUnitAssertionFailureReportStrategy();

        if (config.UseAllNUnitFeatures)
            builder.UseAllNUnitFeatures();

        if (config.UseSpecFlowNUnitFeatures)
            builder.UseSpecFlowNUnitFeatures();

        if (config.LogConsumers is not null)
        {
            foreach (var item in config.LogConsumers)
                MapLogConsumer(item, builder);
        }

        if (config.Drivers is not null)
        {
            foreach (var item in config.Drivers)
                MapDriver(item, builder);
        }

        if (config.Attributes is not null)
            MapAttributes(config.Attributes, builder);

        if (config.EventSubscriptions is not null)
            MapEventSubscriptions(config.EventSubscriptions, builder);

        if (config.Screenshots is not null)
            MapScreenshots(config.Screenshots, builder);

        if (config.PageSnapshots is not null)
            MapPageSnapshots(config.PageSnapshots, builder);

        if (config.BrowserLogs is not null)
            MapBrowserLogs(config.BrowserLogs, builder);

        if (warnings.Count > 0)
            builder.EventSubscriptions.Add(new LogConfigurationWarningsEventHandler(warnings));

        return builder;
    }

    private static void MapLogConsumer(LogConsumerJsonSection section, AtataContextBuilder builder)
    {
        var consumerBuilder = builder.LogConsumers.Add(section.Type);

        if (section.MinLevel != null)
            consumerBuilder.WithMinLevel(section.MinLevel.Value);

        if (section.SectionEnd != null)
            consumerBuilder.WithSectionEnd(section.SectionEnd.Value);

        if (section.MessageNestingLevelIndent != null)
            consumerBuilder.WithMessageNestingLevelIndent(section.MessageNestingLevelIndent);

        if (section.MessageStartSectionPrefix != null)
            consumerBuilder.WithMessageStartSectionPrefix(section.MessageStartSectionPrefix);

        if (section.MessageEndSectionPrefix != null)
            consumerBuilder.WithMessageEndSectionPrefix(section.MessageEndSectionPrefix);

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

    private static void MapEventSubscriptions(List<EventSubscriptionJsonSection> sections, AtataContextBuilder builder)
    {
        EventSubscriptionMapper eventSubscriptionMapper = new EventSubscriptionMapper(
            builder.BuildingContext.AssemblyNamePatternToFindEventTypes ?? builder.BuildingContext.DefaultAssemblyNamePatternToFindTypes,
            builder.BuildingContext.AssemblyNamePatternToFindEventHandlerTypes ?? builder.BuildingContext.DefaultAssemblyNamePatternToFindTypes,
            builder.BuildingContext.DefaultAssemblyNamePatternToFindTypes);

        builder.BuildingContext.EventSubscriptions.AddRange(
            sections.Select(eventSubscriptionMapper.Map));
    }

    private static void MapScreenshots(ScreenshotsJsonSection section, AtataContextBuilder builder)
    {
        if (!string.IsNullOrEmpty(section.FileNameTemplate))
            builder.Screenshots.UseFileNameTemplate(section.FileNameTemplate);

        if (section.Strategy?.Type != null)
        {
            if (!ScreenshotStrategyAliases.TryResolve(section.Strategy.Type, out IScreenshotStrategy strategy))
                strategy = (IScreenshotStrategy)CreateObject(
                    section.Strategy.Type,
                    section.Strategy.ExtraPropertiesMap,
                    builder.BuildingContext.DefaultAssemblyNamePatternToFindTypes);

            builder.Screenshots.UseStrategy(strategy);
        }
    }

    private static void MapPageSnapshots(PageSnapshotsJsonSection section, AtataContextBuilder builder)
    {
        if (!string.IsNullOrEmpty(section.FileNameTemplate))
            builder.PageSnapshots.UseFileNameTemplate(section.FileNameTemplate);

        if (section.Strategy?.Type != null)
        {
            if (!PageSnapshotStrategyAliases.TryResolve(section.Strategy.Type, out IPageSnapshotStrategy strategy))
                strategy = (IPageSnapshotStrategy)CreateObject(
                    section.Strategy.Type,
                    section.Strategy.ExtraPropertiesMap,
                    builder.BuildingContext.DefaultAssemblyNamePatternToFindTypes);

            builder.PageSnapshots.UseStrategy(strategy);
        }
    }

    private static void MapBrowserLogs(BrowserLogsJsonSection section, AtataContextBuilder builder)
    {
        if (section.Log)
            builder.BrowserLogs.UseLog(section.Log);

        if (section.MinLevelOfWarning is not null)
            builder.BrowserLogs.UseMinLevelOfWarning(section.MinLevelOfWarning);
    }

    private static object CreateObject(string typeName, Dictionary<string, object> valuesMap, string assemblyNamePatternToFindType)
    {
        IObjectConverter objectConverter = new ObjectConverter
        {
            AssemblyNamePatternToFindTypes = assemblyNamePatternToFindType
        };
        IObjectMapper objectMapper = new ObjectMapper(objectConverter);
        IObjectCreator objectCreator = new ObjectCreator(objectConverter, objectMapper);

        var assembliesToFindTypes = AssemblyFinder.FindAllByPattern(assemblyNamePatternToFindType);
        Type strategyType = TypeFinder.FindInAssemblies(typeName, assembliesToFindTypes);
        return objectCreator.Create(strategyType, valuesMap);
    }
}
