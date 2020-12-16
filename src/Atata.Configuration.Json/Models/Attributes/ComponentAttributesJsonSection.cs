namespace Atata.Configuration.Json
{
    public class ComponentAttributesJsonSection
    {
        public string Type { get; set; }

        public AttributeJsonSection[] Attributes { get; set; }

        public PropertyAttributesJsonSection[] Properties { get; set; }
    }
}
