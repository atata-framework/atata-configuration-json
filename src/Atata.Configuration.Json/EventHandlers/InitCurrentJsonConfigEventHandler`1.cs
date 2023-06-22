namespace Atata.Configuration.Json;

internal sealed class InitCurrentJsonConfigEventHandler<TConfig> : IEventHandler<AtataContextInitStartedEvent>
    where TConfig : JsonConfig<TConfig>
{
    public void Handle(AtataContextInitStartedEvent eventData, AtataContext context) =>
        JsonConfigManager<TConfig>.InitCurrentValue();
}
