namespace Atata.Configuration.Json;

public class LogConsumerJsonSection : JsonSection
{
    public string Type { get; set; }

    public LogLevel? MinLevel { get; set; }

    [Obsolete($"Use {nameof(SectionEnd)} instead.")] // Obsolete since v2.6.0.
    public bool? SectionFinish { get; set; }

    public LogSectionEndOption? SectionEnd { get; set; }

    public string MessageNestingLevelIndent { get; set; }

    public string MessageStartSectionPrefix { get; set; }

    public string MessageEndSectionPrefix { get; set; }
}
