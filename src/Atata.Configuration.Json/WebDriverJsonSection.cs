using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Atata
{
    public class WebDriverJsonSection : JsonSection
    {
        [JsonProperty("type")]
        public string TypeName { get; set; }

        public Dictionary<string, object> Capabilities { get; set; }

        public string[] Arguments { get; set; }

        public string DriverPath { get; set; }

        public string DriverExecutableFileName { get; set; }

        public TimeSpan CommandTimeout { get; set; }
    }
}
