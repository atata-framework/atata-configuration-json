using System.Collections.Generic;
using OpenQA.Selenium.Chromium;

namespace Atata.Configuration.Json
{
    public class DriverOptionsJsonSection : JsonSection
    {
        public string Type { get; set; }

        public Dictionary<string, OpenQA.Selenium.LogLevel> LoggingPreferences { get; set; }

        public JsonSection AdditionalOptions { get; set; }

        // Chrome, Firefox, Edge, InternetExplorer and Opera specific.
        public JsonSection AdditionalBrowserOptions { get; set; }

        public ProxyJsonSection Proxy { get; set; }

        // Chrome, Firefox, Edge and Opera specific.
        public string[] Arguments { get; set; }

        // Chrome, Edge and Opera specific.
        public string[] ExcludedArguments { get; set; }

        // Chrome, Edge and Opera specific.
        public string[] Extensions { get; set; }

        // Chrome, Edge and Opera specific.
        public string[] EncodedExtensions { get; set; }

        // Chrome and Edge specific.
        public string[] WindowTypes { get; set; }

        // Chrome and Edge specific.
        public DriverPerformanceLoggingPreferencesJsonSection PerformanceLoggingPreferences { get; set; }

        // Chrome, Edge and Opera specific.
        public JsonSection UserProfilePreferences { get; set; }

        // Chrome, Edge and Opera specific.
        public JsonSection LocalStatePreferences { get; set; }

        // Firefox specific.
        public DriverProfileJsonSection Profile { get; set; }

        // Firefox specific.
        public JsonSection Preferences { get; set; }

        // Chrome and Edge specific.
        public string MobileEmulationDeviceName { get; set; }

        // Chrome and Edge specific.
        public ChromiumMobileEmulationDeviceSettings MobileEmulationDeviceSettings { get; set; }

        // Chrome, Firefox and Edge specific.
        public AndroidOptionsJsonSection AndroidOptions { get; set; }
    }
}
