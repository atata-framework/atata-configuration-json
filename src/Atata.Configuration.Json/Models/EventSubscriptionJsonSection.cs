namespace Atata.Configuration.Json;

public class EventSubscriptionJsonSection : JsonSection
{
    public string EventType { get; set; }

    public string HandlerType { get; set; }
}
