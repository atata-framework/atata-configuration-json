namespace Atata.Configuration.Json.Tests;

public class EventSubscriptionsTests : TestFixture
{
    [Test]
    public void Empty() =>
        AtataContext.Configure().ToSubject()
            .Invoking(x => x.ApplyJsonConfig(BuildConfigPath(nameof(Empty)), null))
            .Should.Throw<ConfigurationException>();

    [Test]
    public void EventTypeAndHandlerType() =>
        CreateSutForConfig(nameof(EventTypeAndHandlerType))
            .Should.ContainSingle(x => x.EventType == typeof(TestEvent) && x.EventHandler is TestEventHandler);

    [Test]
    public void HandlerType() =>
        CreateSutForConfig(nameof(HandlerType))
            .Should.ContainSingle(x => x.EventType == typeof(TestEvent) && x.EventHandler is TestEventHandler);

    [Test]
    public void HandlerType_Invalid() =>
        AtataContext.Configure().ToSubject()
            .Invoking(x => x.ApplyJsonConfig(BuildConfigPath(nameof(HandlerType_Invalid)), null))
            .Should.Throw<ConfigurationException>();

    [Test]
    public void EventTypeAndHandlerTypeAndArgument() =>
        CreateSutForConfig(nameof(EventTypeAndHandlerTypeAndArgument))
            .Should.ContainSingle(x => x.EventType == typeof(TestEvent) && x.EventHandler is TestEventHandler && ((TestEventHandler)x.EventHandler).SomeName == "TestName");

    private static Subject<List<EventSubscriptionItem>> CreateSutForConfig(string configName)
    {
        AtataContextBuilder builder = AtataContext.Configure().
            ApplyJsonConfig(BuildConfigPath(configName));

        return builder.BuildingContext.EventSubscriptions.ToSutSubject();
    }

    private static string BuildConfigPath(string configName) =>
        $"Configs/EventSubscriptions/{configName}";

    public class TestEvent
    {
    }

    public class TestEventHandler : IEventHandler<TestEvent>
    {
        public TestEventHandler(string someName = null) =>
            SomeName = someName;

        public string SomeName { get; }

        public void Handle(TestEvent eventData, AtataContext context)
        {
            // Method intentionally left empty.
        }
    }
}
