namespace Atata.Configuration.Json
{
    public class DesiredCapabilitiesJsonSection : JsonSection
    {
        public string Type { get; set; }

        public ProxyJsonSection Proxy { get; set; }
    }
}
