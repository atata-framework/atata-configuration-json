namespace Atata.Configuration.Json
{
    internal sealed class ResetCurrentJsonConfigEventHandler<TConfig> : IEventHandler<AtataContextCleanUpEvent>
        where TConfig : JsonConfig<TConfig>
    {
        public void Handle(AtataContextCleanUpEvent eventData, AtataContext context) =>
            JsonConfigManager<TConfig>.ResetCurrentValue();
    }
}
