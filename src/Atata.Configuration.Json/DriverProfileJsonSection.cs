namespace Atata
{
    public class DriverProfileJsonSection : JsonSection
    {
        public string ProfileDirectory { get; set; }

        public bool? DeleteSourceOnClean { get; set; }

        public JsonSection Preferences { get; set; }

        public string[] Extensions { get; set; }
    }
}
