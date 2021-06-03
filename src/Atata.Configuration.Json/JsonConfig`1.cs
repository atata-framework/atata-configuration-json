using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Atata.Configuration.Json
{
    /// <summary>
    /// Represents JSON configuration.
    /// </summary>
    /// <typeparam name="TConfig">The type of the configuration class.</typeparam>
    public abstract class JsonConfig<TConfig> : JsonSection
        where TConfig : JsonConfig<TConfig>
    {
#if NET46 || NETSTANDARD2_0
        private static readonly System.Threading.AsyncLocal<TConfig> CurrentAsyncLocalConfig = new System.Threading.AsyncLocal<TConfig>();

#endif
        [ThreadStatic]
        private static TConfig currentThreadStaticConfig;

        private static TConfig currentStaticConfig;

        /// <summary>
        /// Gets or sets the global <see cref="JsonConfig{TConfig}"/> instance.
        /// </summary>
        public static TConfig Global { get; set; }

        /// <summary>
        /// Gets or sets the current <see cref="JsonConfig{TConfig}"/> instance.
        /// Keeps in sync with <see cref="AtataContext.Current"/> relying on its <see cref="AtataContext.ModeOfCurrent"/> value.
        /// </summary>
        public static TConfig Current
        {
            get
            {
                return AtataContext.ModeOfCurrent == AtataContextModeOfCurrent.ThreadStatic
                    ? currentThreadStaticConfig
#if NET46 || NETSTANDARD2_0
                    : AtataContext.ModeOfCurrent == AtataContextModeOfCurrent.AsyncLocal
                    ? CurrentAsyncLocalConfig.Value
#endif
                    : currentStaticConfig;
            }

            set
            {
                if (AtataContext.ModeOfCurrent == AtataContextModeOfCurrent.ThreadStatic)
                    currentThreadStaticConfig = value;
#if NET46 || NETSTANDARD2_0
                else if (AtataContext.ModeOfCurrent == AtataContextModeOfCurrent.AsyncLocal)
                    CurrentAsyncLocalConfig.Value = value;
#endif
                else
                    currentStaticConfig = value;
            }
        }

        public List<DriverJsonSection> Drivers { get; set; }

        [JsonConverter(typeof(JsonConverterWithoutPopulation))]
        public DriverJsonSection Driver
        {
            get
            {
                return Drivers?.LastOrDefault();
            }

            set
            {
                if (value != null)
                {
                    if (Drivers == null)
                        Drivers = new List<DriverJsonSection>();

                    Drivers.Add(value);
                }
            }
        }

        public List<LogConsumerJsonSection> LogConsumers { get; set; }

        public List<ScreenshotConsumerJsonSection> ScreenshotConsumers { get; set; }

        public string BaseUrl { get; set; }

        public string Culture { get; set; }

        /// <summary>
        /// Gets or sets the Artifacts directory path.
        /// </summary>
        public string ArtifactsPath { get; set; }

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

        public bool LogNUnitError { get; set; }

        public bool TakeScreenshotOnNUnitError { get; set; }

        public string TakeScreenshotOnNUnitErrorTitle { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use <see cref="NUnitAggregateAssertionStrategy"/> as the aggregate assertion strategy.
        /// </summary>
        public bool UseNUnitAggregateAssertionStrategy { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use <see cref="NUnitWarningReportStrategy"/> as the strategy for warning assertion reporting.
        /// </summary>
        public bool UseNUnitWarningReportStrategy { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to enable all NUnit features for Atata.
        /// </summary>
        public bool UseAllNUnitFeatures { get; set; }

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
        /// Gets or sets the attributes.
        /// </summary>
        public AttributesJsonSection Attributes { get; set; }
    }
}
