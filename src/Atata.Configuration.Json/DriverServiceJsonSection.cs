namespace Atata
{
    public class DriverServiceJsonSection : JsonSection
    {
        public string DriverPath { get; set; }

        public string DriverExecutableFileName { get; set; }

        // PhantomJS specific.
        public string[] Arguments { get; set; }
    }
}
