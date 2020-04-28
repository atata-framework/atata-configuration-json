using System.Collections.Generic;
using OpenQA.Selenium.Chrome;

namespace Atata.Configuration.Json
{
    public class DriverOptionsJsonSection : JsonSection
    {
        public string Type { get; set; }

        // Common, but actually used only by Chrome.
        public Dictionary<string, OpenQA.Selenium.LogLevel> LoggingPreferences { get; set; }

        public JsonSection AdditionalCapabilities { get; set; }

        // Chrome, Firefox, InternetExplorer and Opera specific.
        public JsonSection GlobalAdditionalCapabilities { get; set; }

        // Chrome, Firefox, Opera and Internet Explorer specific.
        public ProxyJsonSection Proxy { get; set; }

        // Chrome, Firefox and Opera specific.
        public string[] Arguments { get; set; }

        // Chrome and Opera specific.
        public string[] ExcludedArguments { get; set; }

        // Chrome, Edge and Opera specific.
        public string[] Extensions { get; set; }

        // Chrome and Opera specific.
        public string[] EncodedExtensions { get; set; }

        // Chrome specific.
        public string[] WindowTypes { get; set; }

        // Chrome specific.
        public DriverPerformanceLoggingPreferencesJsonSection PerformanceLoggingPreferences { get; set; }

        // Chrome and Opera specific.
        public JsonSection UserProfilePreferences { get; set; }

        // Chrome and Opera specific.
        public JsonSection LocalStatePreferences { get; set; }

        // Firefox specific.
        public DriverProfileJsonSection Profile { get; set; }

        // Firefox specific.
        public JsonSection Preferences { get; set; }

        // Chrome specific.
        public string MobileEmulationDeviceName { get; set; }

        // Chrome specific.
        public ChromeMobileEmulationDeviceSettings MobileEmulationDeviceSettings { get; set; }
    }
}
