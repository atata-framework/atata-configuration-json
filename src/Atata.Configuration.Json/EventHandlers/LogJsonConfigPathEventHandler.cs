namespace Atata.Configuration.Json;

internal sealed class LogJsonConfigPathEventHandler : IEventHandler<AtataContextInitStartedEvent>
{
    private readonly string _configPath;

    internal LogJsonConfigPathEventHandler(string configPath) =>
        _configPath = configPath;

    public void Handle(AtataContextInitStartedEvent eventData, AtataContext context) =>
        context.Log.Trace($"Use: \"{_configPath}\" config");
}
