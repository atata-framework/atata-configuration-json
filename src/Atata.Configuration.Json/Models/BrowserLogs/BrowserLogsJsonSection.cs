namespace Atata.Configuration.Json;

public sealed class BrowserLogsJsonSection
{
    public bool Log { get; set; }

    public LogLevel? MinLevelOfWarning { get; set; }
}
