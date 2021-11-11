namespace Atata.Configuration.Json
{
    internal sealed class InitCurrentJsonConfigEventHandler<TConfig> : IEventHandler<AtataContextInitEvent>
        where TConfig : JsonConfig<TConfig>
    {
        public void Handle(AtataContextInitEvent eventData, AtataContext context) =>
            JsonConfigManager<TConfig>.InitCurrentValue();
    }
}
