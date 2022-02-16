namespace Atata.Configuration.Json
{
    public class AndroidOptionsJsonSection : JsonSection
    {
        public string AndroidPackage { get; set; }

        // Firefox specific.
        public string[] AndroidIntentArguments { get; set; }
    }
}
