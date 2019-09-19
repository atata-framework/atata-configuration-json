using System;
using System.Linq;

namespace Atata.Configuration.Json
{
    /// <summary>
    /// Represents JSON configuration.
    /// </summary>
    /// <typeparam name="TConfig">The type of the configuration class.</typeparam>
    public abstract class JsonConfig<TConfig> : JsonSection
        where TConfig : JsonConfig<TConfig>
    {
        [ThreadStatic]
#pragma warning disable S2743 // Static fields should not be used in generic types
        private static TConfig currentThreadStaticConfig;

        private static TConfig currentStaticConfig;
#pragma warning restore S2743 // Static fields should not be used in generic types

        /// <summary>
        /// Gets or sets the global <see cref="JsonConfig{TConfig}"/> instance.
        /// </summary>
        public static TConfig Global { get; set; }

        /// <summary>
        /// Gets or sets the current <see cref="JsonConfig{TConfig}"/> instance.
        /// </summary>
        public static TConfig Current
        {
            get => AtataContext.IsThreadStatic ? currentThreadStaticConfig : currentStaticConfig;
            set
            {
                if (AtataContext.IsThreadStatic)
                    currentThreadStaticConfig = value;
                else
                    currentStaticConfig = value;
            }
        }

        public DriverJsonSection[] Drivers { get; set; }

        public DriverJsonSection Driver
        {
            get { return Drivers?.FirstOrDefault(); }
            set { Drivers = value == null ? null : new[] { value }; }
        }

        public LogConsumerJsonSection[] LogConsumers { get; set; }

        public ScreenshotConsumerJsonSection[] ScreenshotConsumers { get; set; }

        public string BaseUrl { get; set; }

        public string Culture { get; set; }

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
        /// Gets or sets the assembly-qualified type name of the assertion exception.
        /// </summary>
        public string AssertionExceptionType { get; set; }

        /// <summary>
        /// Gets or sets the assembly-qualified type name of the aggregate assertion exception.
        /// The exception type should have public constructor with <c>IEnumerable&lt;AssertionResult&gt;</c> argument.
        /// </summary>
        public string AggregateAssertionExceptionType { get; set; }

        public bool UseNUnitTestName { get; set; }

        public bool LogNUnitError { get; set; }

        public bool TakeScreenshotOnNUnitError { get; set; }

        public string TakeScreenshotOnNUnitErrorTitle { get; set; }
    }
}
