using OpenQA.Selenium;

namespace Atata
{
    public class CapabilitiesJsonSection : ObjectDictionaryJsonSection
    {
        public PlatformType? Platform { get; set; }
    }
}
