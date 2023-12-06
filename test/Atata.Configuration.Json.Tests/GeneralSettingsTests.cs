namespace Atata.Configuration.Json.Tests;

[TestFixture]
public class GeneralSettingsTests : TestFixture
{
    [Test]
    public void GeneralAndNUnit()
    {
        AtataContextBuilder builder = AtataContext.Configure()
            .ApplyJsonConfig("Configs/Chrome+NUnit.json");

        var context = builder.BuildingContext;

        using (new AssertionScope())
        {
            context.BaseUrl.Should().Be("https://demo.atata.io/");

            context.DefaultControlVisibility.Should().Be(Visibility.Visible);

            context.Culture.Name.Should().Be("en-US");

            context.EventSubscriptions.Where(x => x.EventType == typeof(AtataContextDeInitEvent))
                .Should().HaveCount(3);
            context.EventSubscriptions.Where(x => x.EventType == typeof(AtataContextDeInitCompletedEvent))
                .Should().HaveCount(2);
            context.EventSubscriptions.Should().Contain(x => x.EventHandler is LogNUnitErrorEventHandler);
            context.EventSubscriptions.Should().Contain(x => x.EventHandler is TakeScreenshotOnNUnitErrorEventHandler);
            context.EventSubscriptions.Should().Contain(x => x.EventHandler is TakePageSnapshotOnNUnitErrorEventHandler);
            context.EventSubscriptions.Should().Contain(x => x.EventHandler is AddArtifactsToNUnitTestContextEventHandler);

            context.AssertionExceptionType.Should().Be(typeof(NUnit.Framework.AssertionException));

            context.BaseRetryTimeout.Should().Be(TimeSpan.FromSeconds(7));
            context.BaseRetryInterval.Should().Be(TimeSpan.FromSeconds(0.7));

            context.ElementFindTimeout.Should().Be(TimeSpan.FromSeconds(8));
            context.ElementFindRetryInterval.Should().Be(TimeSpan.FromSeconds(0.8));

            context.WaitingTimeout.Should().Be(TimeSpan.FromSeconds(9));
            context.WaitingRetryInterval.Should().Be(TimeSpan.FromSeconds(0.9));

            context.VerificationTimeout.Should().Be(TimeSpan.FromSeconds(10));
            context.VerificationRetryInterval.Should().Be(TimeSpan.FromSeconds(1));

            context.TestNameFactory().Should().Be(nameof(GeneralAndNUnit));
            context.TestSuiteNameFactory().Should().Be(nameof(GeneralSettingsTests));
            context.TestSuiteTypeFactory().Should().Be(typeof(GeneralSettingsTests));

            context.DefaultAssemblyNamePatternToFindTypes.Should().Be("def");
            context.AssemblyNamePatternToFindComponentTypes.Should().Be("comp");
            context.AssemblyNamePatternToFindAttributeTypes.Should().Be("attr");

            context.Screenshots.Strategy.Should().BeOfType<FullPageOrViewportScreenshotStrategy>();

            context.PageSnapshots.FileNameTemplate.Should().Be("{snapshot-number:D2}!");
            context.PageSnapshots.Strategy.Should().BeOfType<PageSourcePageSnapshotStrategy>();
        }
    }

    [Test]
    public void ObsoleteNUnitProperties()
    {
        List<string> logEntries = [];

        using var context = AtataContext.Configure()
            .UseDriverInitializationStage(AtataContextDriverInitializationStage.None)
            .ApplyJsonConfig("Configs/NUnitObsoleteProperties.json")
            .LogConsumers.Add(new TextOutputLogConsumer(logEntries.Add))
                .WithMinLevel(LogLevel.Warn)
            .Build();

        logEntries.Should().HaveCount(4);
        logEntries[0].Should().Contain("\"logNUnitError\" configuration property is deprecated");
        logEntries[1].Should().Contain("\"takeScreenshotOnNUnitError\", \"takeScreenshotOnNUnitErrorKind\" and \"takeScreenshotOnNUnitErrorTitle\" configuration properties are deprecated");
        logEntries[2].Should().Contain("\"takePageSnapshotOnNUnitError\" and \"takePageSnapshotOnNUnitErrorTitle\" configuration properties are deprecated");
        logEntries[3].Should().Contain("\"onCleanUpAddArtifactsToNUnitTestContext\" configuration property is deprecated");
    }
}
