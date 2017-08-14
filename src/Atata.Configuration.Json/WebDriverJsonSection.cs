using Newtonsoft.Json;

namespace Atata
{
    public class WebDriverJsonSection : JsonSection
    {
        public string Alias { get; set; }

        [JsonProperty("type")]
        public string TypeName { get; set; }

        public string RemoteAddress { get; set; }

        public DriverOptionsJsonSection Options { get; set; }

        public DesiredCapabilitiesKind DesiredCapabilities { get; set; }

        public JsonSection Capabilities { get; set; }

        public string DriverPath { get; set; }

        public string DriverExecutableFileName { get; set; }

        /// <summary>
        /// Gets or sets the command timeout in seconds.
        /// </summary>
        public double? CommandTimeout { get; set; }
    }
}
