namespace Atata
{
    public class DesiredCapabilitiesJsonSection : CapabilitiesJsonSection
    {
        public string Type { get; set; }

        public ProxyJsonSection Proxy { get; set; }
    }
}
