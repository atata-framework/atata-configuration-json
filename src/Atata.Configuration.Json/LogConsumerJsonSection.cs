namespace Atata
{
    public class LogConsumerJsonSection : JsonSection
    {
        public string TypeName { get; set; }

        public LogLevel MinLevel { get; set; }

        public bool SectionFinish { get; set; }
    }
}
