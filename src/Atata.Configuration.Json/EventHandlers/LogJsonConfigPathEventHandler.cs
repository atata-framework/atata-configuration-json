namespace Atata.Configuration.Json
{
    internal sealed class LogJsonConfigPathEventHandler : IEventHandler<AtataContextInitCompletedEvent>
    {
        private readonly string _configPath;

        internal LogJsonConfigPathEventHandler(string configPath)
        {
            _configPath = configPath;
        }

        public void Handle(AtataContextInitCompletedEvent eventData, AtataContext context) =>
            context.Log.Trace($"Use: \"{_configPath}\" config");
    }
}
