using System;

namespace Atata
{
    public abstract class JsonConfig<TConfig> : JsonSection
        where TConfig : JsonConfig<TConfig>
    {
        [ThreadStatic]
        private static TConfig current;

        public static TConfig Current
        {
            get { return current; }
            set { current = value; }
        }

        public string Driver { get; set; }

        public string UseNUnitTestName { get; set; }

        public LogConsumerJsonSection LogConsumers { get; set; }
    }
}
