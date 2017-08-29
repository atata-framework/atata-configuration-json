using Newtonsoft.Json;

namespace Atata
{
    public class DriverJsonSection : JsonSection
    {
        public string Alias { get; set; }

        public string Type { get; set; }

        public string RemoteAddress { get; set; }

        public DriverOptionsJsonSection Options { get; set; }

        public DriverServiceJsonSection Service { get; set; }

        public DesiredCapabilitiesKind DesiredCapabilities { get; set; }

        public DesiredCapabilitiesJsonSection Capabilities { get; set; }

        /// <summary>
        /// Gets or sets the command timeout in seconds.
        /// </summary>
        public double? CommandTimeout { get; set; }
    }
}
