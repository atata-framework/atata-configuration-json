using System.Collections.Generic;
using OpenQA.Selenium;

namespace Atata
{
    public class DesiredCapabilitiesJsonSection : CapabilitiesJsonSection
    {
        // TODO: Remove.
        public Proxy Proxy { get; set; }

        public Dictionary<string, OpenQA.Selenium.LogLevel> LoggingPreferences { get; set; }
    }
}
