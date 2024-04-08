namespace Atata.Configuration.Json;

public sealed class ScreenshotsJsonSection
{
    public string FileNameTemplate { get; set; }

    public ScreenshotStrategyJsonSection Strategy { get; set; }
}
