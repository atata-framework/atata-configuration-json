namespace Atata
{
    public class DriverProfileJsonSection : JsonSection
    {
        public JsonSection Preferences { get; set; }

        public string[] Extensions { get; set; }
    }
}
