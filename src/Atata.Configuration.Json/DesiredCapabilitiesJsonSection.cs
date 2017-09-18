namespace Atata.Configuration.Json
{
    public class DesiredCapabilitiesJsonSection : CapabilitiesJsonSection
    {
        public string Type { get; set; }

        public ProxyJsonSection Proxy { get; set; }
    }
}
