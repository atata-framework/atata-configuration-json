using FluentAssertions;
using NUnit.Framework;

namespace Atata.Configuration.Json.Tests
{
    [TestFixture]
    public class CustomSettingsTests
    {
        [Test]
        public void CustomSettings()
        {
            AtataContext.Configure().
                ApplyJsonConfig<CustomJsonConfig>(@"Configs/CustomSettings.json");

            CustomJsonConfig.Current.IntProperty.Should().Be(5);
            CustomJsonConfig.Current.StringProperty.Should().Be("str");
            CustomJsonConfig.Current.BoolProperty.Should().Be(true);
            CustomJsonConfig.Current.BoolProperty2.Should().Be(false);
            CustomJsonConfig.Current.StringArrayValues.Should().Equal(new[] { "str1", "str2", "str3" });

            CustomJsonConfig.Current.Section.StringProperty.Should().Be("section_str");
            CustomJsonConfig.Current.Section.BoolProperty.Should().Be(true);

            CustomJsonConfig.Current.Items.ShouldBeEquivalentTo(new[]
            {
                new CustomJsonConfig.CustomItemSection
                {
                    Name = "item1",
                    Value = 5
                },
                new CustomJsonConfig.CustomItemSection
                {
                    Name = "item2",
                    Value = 7
                }
            });
        }
    }
}
