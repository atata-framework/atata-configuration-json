using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Atata
{
    public class DriverOptionsJsonSection : JsonSection
    {
        public Dictionary<string, OpenQA.Selenium.LogLevel> LoggingPreferences { get; set; }

        // TODO: Remove.
        public Proxy Proxy { get; set; }

        public CapabilitiesJsonSection AdditionalCapabilities { get; set; }

        public CapabilitiesJsonSection GlobalAdditionalCapabilities { get; set; }

        // Chrome, Firefox and Opera specific.
        public string[] Arguments { get; set; }

        // Chrome and Opera specific.
        public string[] ExcludedArguments { get; set; }

        // Chrome and Opera specific.
        public string[] Extensions { get; set; }

        // Chrome and Opera specific.
        public string[] EncodedExtensions { get; set; }

        // Chrome specific.
        public string[] WindowTypes { get; set; }

        // Chrome and Opera specific.
        public ObjectDictionaryJsonSection UserProfilePreferences { get; set; }

        // Chrome and Opera specific.
        public ObjectDictionaryJsonSection LocalStatePreferences { get; set; }

        // Firefox specific.
        public DriverProfileJsonSection Profile { get; set; }

        // Firefox specific.
        public ObjectDictionaryJsonSection Preferences { get; set; }

        // Chrome specific.
        public string MobileEmulationDeviceName { get; set; }

        // Chrome specific.
        public ChromeMobileEmulationDeviceSettings MobileEmulationDeviceSettings { get; set; }
    }
}
