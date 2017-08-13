using Newtonsoft.Json;

namespace Atata
{
    public class LogConsumerJsonSection : JsonSection
    {
        [JsonProperty("type")]
        public string TypeName { get; set; }

        public LogLevel MinLevel { get; set; }

        public bool SectionFinish { get; set; }
    }
}
