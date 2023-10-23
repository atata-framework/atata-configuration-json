namespace Atata.Configuration.Json;

internal sealed class LogConfigurationWarningsEventHandler : IEventHandler<AtataContextInitStartedEvent>
{
    private readonly IEnumerable<string> _warningMessages;

    internal LogConfigurationWarningsEventHandler(IEnumerable<string> warningMessages) =>
        _warningMessages = warningMessages;

    public void Handle(AtataContextInitStartedEvent eventData, AtataContext context)
    {
        foreach (string warningMessage in _warningMessages)
            context.Log.Warn(warningMessage);
    }
}
