namespace Atata.Configuration.Json.Tests;

public class CustomJsonConfig : JsonConfig<CustomJsonConfig>
{
    public int IntProperty { get; set; }

    public string StringProperty { get; set; }

    public bool BoolProperty { get; set; }

    public bool BoolProperty2 { get; set; }

    public string[] StringArrayValues { get; set; }

    public List<string> StringListValues { get; } = new List<string>();

    public CustomSection Section { get; set; }

    public List<CustomItemSection> Items { get; } = new List<CustomItemSection>();

    public class CustomSection
    {
        public string StringProperty { get; set; }

        public bool BoolProperty { get; set; }
    }

    public class CustomItemSection
    {
        public string Name { get; set; }

        public int Value { get; set; }
    }
}
