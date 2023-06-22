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

            context.EventSubscriptions.Where(x => x.EventType == typeof(AtataContextCleanUpEvent))
                .Should().HaveCount(3);

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

            context.DefaultAssemblyNamePatternToFindTypes.Should().Be("def");
            context.AssemblyNamePatternToFindComponentTypes.Should().Be("comp");
            context.AssemblyNamePatternToFindAttributeTypes.Should().Be("attr");

            context.EventSubscriptions.Should().Contain(x => x.EventHandler is TakeScreenshotOnNUnitErrorOnCleanUpEventHandler);
            context.Screenshots.Strategy.Should().BeOfType<FullPageOrViewportScreenshotStrategy>();

            context.PageSnapshots.FileNameTemplate.Should().Be("{snapshot-number:D2}!");
            context.PageSnapshots.Strategy.Should().BeOfType<PageSourcePageSnapshotStrategy>();
        }
    }
}
