namespace Atata.Configuration.Json.Tests;

public sealed class BrowserLogsTests : TestFixture
{
    [Test]
    public void WithAllPropertiesSet()
    {
        var builder = AtataContext.Configure().
            ApplyJsonConfig($"Configs/BrowserLogs");

        builder.BuildingContext.ToResultSubject()
            .ValueOf(x => x.BrowserLogs.Log).Should.BeTrue()
            .ValueOf(x => x.BrowserLogs.MinLevelOfWarning).Should.Be(LogLevel.Warn);
    }
}
