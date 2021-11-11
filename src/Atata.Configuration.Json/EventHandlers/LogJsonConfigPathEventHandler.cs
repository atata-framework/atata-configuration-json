namespace Atata.Configuration.Json
{
    internal sealed class LogJsonConfigPathEventHandler : IEventHandler<AtataContextInitEvent>
    {
        private readonly string _configPath;

        internal LogJsonConfigPathEventHandler(string configPath)
        {
            _configPath = configPath;
        }

        public void Handle(AtataContextInitEvent eventData, AtataContext context) =>
            context.Log.Trace($"Use: \"{_configPath}\" config");
    }
}
