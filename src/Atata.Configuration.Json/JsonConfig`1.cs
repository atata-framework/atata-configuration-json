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
            get { return Drivers?.SingleOrDefault(); }
            set { Drivers = new[] { value }; }
        }

        public LogConsumerJsonSection[] LogConsumers { get; set; }

        public ScreenshotConsumerJsonSection[] ScreenshotConsumers { get; set; }

        public string BaseUrl { get; set; }

        /// <summary>
        /// Gets or sets the retry timeout in seconds.
        /// </summary>
        public double? RetryTimeout { get; set; }

        /// <summary>
        /// Gets or sets the retry interval in seconds.
        /// </summary>
        public double? RetryInterval { get; set; }

        public string AssertionExceptionType { get; set; }

        public bool UseNUnitTestName { get; set; }

        public bool LogNUnitError { get; set; }

        public bool TakeScreenshotOnNUnitError { get; set; }

        public string TakeScreenshotOnNUnitErrorTitle { get; set; }
    }
}
