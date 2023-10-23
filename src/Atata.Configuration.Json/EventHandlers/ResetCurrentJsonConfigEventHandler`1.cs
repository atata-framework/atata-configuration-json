namespace Atata.Configuration.Json;

internal sealed class ResetCurrentJsonConfigEventHandler<TConfig> : IEventHandler<AtataContextDeInitCompletedEvent>
    where TConfig : JsonConfig<TConfig>
{
    public void Handle(AtataContextDeInitCompletedEvent eventData, AtataContext context) =>
        JsonConfigManager<TConfig>.ResetCurrentValue();
}
