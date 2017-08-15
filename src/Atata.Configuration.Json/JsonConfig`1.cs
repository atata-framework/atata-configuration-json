using System;
using System.Linq;

namespace Atata
{
    public abstract class JsonConfig<TConfig> : JsonSection
        where TConfig : JsonConfig<TConfig>
    {
        [ThreadStatic]
#pragma warning disable S2743 // Static fields should not be used in generic types
        private static TConfig current;
#pragma warning restore S2743 // Static fields should not be used in generic types

        /// <summary>
        /// Gets or sets the current <see cref="JsonConfig{TConfig}"/> instance.
        /// </summary>
        public static TConfig Current
        {
            get { return current; }
            set { current = value; }
        }

        public DriverJsonSection[] Drivers { get; set; }

        public DriverJsonSection Driver
        {
            get { return Drivers?.SingleOrDefault(); }
            set { Drivers = new[] { value }; }
        }

        public LogConsumerJsonSection[] LogConsumers { get; set; }

        /// <summary>
        /// Gets or sets the retry timeout in seconds.
        /// </summary>
        public double? RetryTimeout { get; set; }

        /// <summary>
        /// Gets or sets the retry interval in seconds.
        /// </summary>
        public double? RetryInterval { get; set; }

        public bool UseNUnitTestName { get; set; }

        public bool LogNUnitError { get; set; }
    }
}
