namespace Atata.Configuration.Json
{
    public class DriverJsonSection : JsonSection
    {
        public string Alias { get; set; }

        public string Type { get; set; }

        public string RemoteAddress { get; set; }

        public DriverOptionsJsonSection Options { get; set; }

        public DriverServiceJsonSection Service { get; set; }

        /// <summary>
        /// Gets or sets the command timeout in seconds.
        /// </summary>
        public double? CommandTimeout { get; set; }
    }
}
