namespace Atata.Configuration.Json
{
    public class LogConsumerJsonSection : JsonSection
    {
        public string Type { get; set; }

        public LogLevel? MinLevel { get; set; }

        public bool? SectionFinish { get; set; }
    }
}
