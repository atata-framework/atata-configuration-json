namespace Atata
{
    public class DriverProfileJsonSection : JsonSection
    {
        public ObjectDictionaryJsonSection Preferences { get; set; }

        public string[] Extensions { get; set; }
    }
}
