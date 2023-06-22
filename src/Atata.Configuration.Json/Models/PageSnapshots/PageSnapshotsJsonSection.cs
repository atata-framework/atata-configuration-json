namespace Atata.Configuration.Json;

public sealed class PageSnapshotsJsonSection
{
    public string FileNameTemplate { get; set; }

    public PageSnapshotStrategyJsonSection Strategy { get; set; }
}
