namespace Atata.Configuration.Json;

/// <summary>
/// Represents JSON configuration.
/// </summary>
/// <typeparam name="TConfig">The type of the configuration class.</typeparam>
public abstract class JsonConfig<TConfig> : JsonSection
    where TConfig : JsonConfig<TConfig>
{
    private static readonly AsyncLocal<TConfig> s_currentAsyncLocalConfig = new();

    [ThreadStatic]
    private static TConfig s_currentThreadStaticConfig;

    private static TConfig s_currentStaticConfig;

    /// <summary>
    /// Gets or sets the global <see cref="JsonConfig{TConfig}"/> instance.
    /// </summary>
    public static TConfig Global { get; set; }

    /// <summary>
    /// Gets or sets the current <see cref="JsonConfig{TConfig}"/> instance.
    /// Relies on <see cref="AtataContextGlobalProperties.ModeOfCurrent"/> value of <see cref="AtataContext.GlobalProperties"/>.
    /// </summary>
    public static TConfig Current
    {
        get => AtataContext.GlobalProperties.ModeOfCurrent == AtataContextModeOfCurrent.AsyncLocal
        ? s_currentAsyncLocalConfig.Value
            : AtataContext.GlobalProperties.ModeOfCurrent == AtataContextModeOfCurrent.ThreadStatic
                ? s_currentThreadStaticConfig
                : s_currentStaticConfig;
        set
        {
            if (AtataContext.GlobalProperties.ModeOfCurrent == AtataContextModeOfCurrent.AsyncLocal)
                s_currentAsyncLocalConfig.Value = value;
            else if (AtataContext.GlobalProperties.ModeOfCurrent == AtataContextModeOfCurrent.ThreadStatic)
                s_currentThreadStaticConfig = value;
            else
                s_currentStaticConfig = value;
        }
    }

    public List<DriverJsonSection> Drivers { get; set; }

    [JsonConverter(typeof(JsonConverterWithoutPopulation))]
    public DriverJsonSection Driver
    {
        get => Drivers?.LastOrDefault();
        set
        {
            if (value != null)
            {
                Drivers ??= [];
                Drivers.Add(value);
            }
        }
    }

    /// <summary>
    /// Gets or sets the driver initialization stage.
    /// </summary>
    public AtataContextDriverInitializationStage? DriverInitializationStage { get; set; }

    public List<LogConsumerJsonSection> LogConsumers { get; set; }

    public string BaseUrl { get; set; }

    public Visibility? DefaultControlVisibility { get; set; }

    public string Culture { get; set; }

    /// <summary>
    /// Gets or sets the Artifacts directory path template.
    /// </summary>
    public string ArtifactsPathTemplate { get; set; }

    /// <summary>
    /// Gets or sets the variables.
    /// </summary>
    public Dictionary<string, object> Variables { get; set; }

    /// <summary>
    /// Gets or sets the base retry timeout in seconds.
    /// </summary>
    [Obsolete("Use BaseRetryTimeout instead.")] // Obsolete since v0.17.0.
    public double? RetryTimeout
    {
        get => BaseRetryTimeout;
        set => BaseRetryTimeout = value;
    }

    /// <summary>
    /// Gets or sets the base retry interval in seconds.
    /// </summary>
    [Obsolete("Use BaseRetryInterval instead.")] // Obsolete since v0.17.0.
    public double? RetryInterval
    {
        get => BaseRetryInterval;
        set => BaseRetryInterval = value;
    }

    /// <summary>
    /// Gets or sets the base retry timeout in seconds.
    /// </summary>
    public double? BaseRetryTimeout { get; set; }

    /// <summary>
    /// Gets or sets the base retry interval in seconds.
    /// </summary>
    public double? BaseRetryInterval { get; set; }

    /// <summary>
    /// Gets or sets the element find timeout in seconds.
    /// </summary>
    public double? ElementFindTimeout { get; set; }

    /// <summary>
    /// Gets or sets the element find retry interval in seconds.
    /// </summary>
    public double? ElementFindRetryInterval { get; set; }

    /// <summary>
    /// Gets or sets the waiting timeout in seconds.
    /// </summary>
    public double? WaitingTimeout { get; set; }

    /// <summary>
    /// Gets or sets the waiting retry interval in seconds.
    /// </summary>
    public double? WaitingRetryInterval { get; set; }

    /// <summary>
    /// Gets or sets the verification timeout in seconds.
    /// </summary>
    public double? VerificationTimeout { get; set; }

    /// <summary>
    /// Gets or sets the verification retry interval in seconds.
    /// </summary>
    public double? VerificationRetryInterval { get; set; }

    /// <summary>
    /// Gets or sets the type name of the assertion exception.
    /// </summary>
    public string AssertionExceptionType { get; set; }

    /// <summary>
    /// Gets or sets the type name of the aggregate assertion exception.
    /// The exception type should have public constructor with <c>IEnumerable&lt;AssertionResult&gt;</c> argument.
    /// </summary>
    public string AggregateAssertionExceptionType { get; set; }

    /// <summary>
    /// Gets or sets the type name of the aggregate assertion strategy.
    /// The type should implement <see cref="IAggregateAssertionStrategy"/>.
    /// </summary>
    public string AggregateAssertionStrategyType { get; set; }

    /// <summary>
    /// Gets or sets the type name of the strategy for warning assertion reporting.
    /// The type should implement <see cref="IWarningReportStrategy"/>.
    /// </summary>
    public string WarningReportStrategyType { get; set; }

    /// <summary>
    /// Gets or sets the type name of the strategy for assertion failure reporting.
    /// The type should implement <see cref="IAssertionFailureReportStrategy"/>.
    /// </summary>
    public string AssertionFailureReportStrategyType { get; set; }

    /// <summary>
    /// Gets or sets the name of the DOM test identifier attribute.
    /// </summary>
    public string DomTestIdAttributeName { get; set; }

    /// <summary>
    /// Gets or sets the default case of the DOM test identifier attribute.
    /// </summary>
    public TermCase? DomTestIdAttributeDefaultCase { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to use NUnit test name.
    /// </summary>
    public bool UseNUnitTestName { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to use NUnit test suite (fixture) name.
    /// </summary>
    public bool UseNUnitTestSuiteName { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to use NUnit test suite (fixture) type.
    /// </summary>
    public bool UseNUnitTestSuiteType { get; set; }

    [Obsolete("Don't use the property as it will be removed in v3.")] // Obsolete since v2.5.0.
    public bool LogNUnitError { get; set; }

    [Obsolete("Don't use the property as it will be removed in v3.")] // Obsolete since v2.5.0.
    public bool TakeScreenshotOnNUnitError { get; set; }

    [Obsolete("Don't use the property as it will be removed in v3.")] // Obsolete since v2.5.0.
    public string TakeScreenshotOnNUnitErrorTitle { get; set; }

    [Obsolete("Don't use the property as it will be removed in v3.")] // Obsolete since v2.5.0.
    public ScreenshotKind? TakeScreenshotOnNUnitErrorKind { get; set; }

    [Obsolete("Don't use the property as it will be removed in v3.")] // Obsolete since v2.5.0.
    public bool TakePageSnapshotOnNUnitError { get; set; }

    [Obsolete("Don't use the property as it will be removed in v3.")] // Obsolete since v2.5.0.
    public string TakePageSnapshotOnNUnitErrorTitle { get; set; }

    [Obsolete("Don't use the property as it will be removed in v3.")] // Obsolete since v2.5.0.
    public bool OnCleanUpAddArtifactsToNUnitTestContext { get; set; }

    // TODO: Make obsolete in v2.6.0.
    public string OnCleanUpAddDirectoryFilesToNUnitTestContext { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to use <see cref="NUnitAggregateAssertionStrategy"/> as the aggregate assertion strategy.
    /// </summary>
    public bool UseNUnitAggregateAssertionStrategy { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to use <see cref="NUnitWarningReportStrategy"/> as the strategy for warning assertion reporting.
    /// </summary>
    public bool UseNUnitWarningReportStrategy { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to use <see cref="NUnitAssertionFailureReportStrategy"/> as the strategy for assertion failure reporting.
    /// </summary>
    public bool UseNUnitAssertionFailureReportStrategy { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to enable all Atata features for NUnit.
    /// </summary>
    public bool UseAllNUnitFeatures { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to enable all Atata features for SpecFlow+NUnit.
    /// </summary>
    public bool UseSpecFlowNUnitFeatures { get; set; }

    /// <summary>
    /// Gets or sets the default assembly name pattern that is used to filter assemblies to find types in them.
    /// </summary>
    public string DefaultAssemblyNamePatternToFindTypes { get; set; }

    /// <summary>
    /// Gets or sets the assembly name pattern that is used to filter assemblies to find component types in them.
    /// </summary>
    public string AssemblyNamePatternToFindComponentTypes { get; set; }

    /// <summary>
    /// Gets or sets the assembly name pattern that is used to filter assemblies to find attribute types in them.
    /// </summary>
    public string AssemblyNamePatternToFindAttributeTypes { get; set; }

    /// <summary>
    /// Gets or sets the assembly name pattern that is used to filter assemblies to find event types in them.
    /// </summary>
    public string AssemblyNamePatternToFindEventTypes { get; set; }

    /// <summary>
    /// Gets or sets the assembly name pattern that is used to filter assemblies to find event handler types in them.
    /// </summary>
    public string AssemblyNamePatternToFindEventHandlerTypes { get; set; }

    /// <summary>
    /// Gets or sets the attributes.
    /// </summary>
    public AttributesJsonSection Attributes { get; set; }

    /// <summary>
    /// Gets or sets the event subscriptions.
    /// </summary>
    public List<EventSubscriptionJsonSection> EventSubscriptions { get; set; }

    /// <summary>
    /// Gets or sets the screenshots configuration.
    /// </summary>
    public ScreenshotsJsonSection Screenshots { get; set; }

    /// <summary>
    /// Gets or sets the page snapshots configuration.
    /// </summary>
    public PageSnapshotsJsonSection PageSnapshots { get; set; }

    /// <summary>
    /// Gets or sets the browser logs configuration.
    /// </summary>
    public BrowserLogsJsonSection BrowserLogs { get; set; }
}
