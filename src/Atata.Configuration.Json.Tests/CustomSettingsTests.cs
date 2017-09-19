using FluentAssertions;
using NUnit.Framework;

namespace Atata.Configuration.Json.Tests
{
    [TestFixture]
    public class CustomSettingsTests : TestFixture
    {
        [Test]
        public void CustomSettings()
        {
            AtataContext.Configure().
                ApplyJsonConfig<CustomJsonConfig>(@"Configs/CustomSettings.json");

            CustomJsonConfig.Current.BaseUrl.Should().Be("https://atata-framework.github.io/atata-sample-app/#!/");
            CustomJsonConfig.Current.IntProperty.Should().Be(5);
            CustomJsonConfig.Current.StringProperty.Should().Be("str");
            CustomJsonConfig.Current.BoolProperty.Should().Be(true);
            CustomJsonConfig.Current.BoolProperty2.Should().Be(false);
            CustomJsonConfig.Current.StringArrayValues.Should().Equal(new[] { "str1", "str2", "str3" });
            CustomJsonConfig.Current.StringListValues.Should().Equal(new[] { "str1", "str2", "str3" });

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

        [Test]
        public void CustomSettings_Merged()
        {
            AtataContext.Configure().
                ApplyJsonConfig<CustomJsonConfig>(@"Configs/CustomSettings.json").
                ApplyJsonConfig<CustomJsonConfig>(@"Configs/CustomSettingsOverride.json");

            CustomJsonConfig.Current.BaseUrl.Should().Be("https://atata-framework.github.io/atata-sample-app/#!/override");
            CustomJsonConfig.Current.IntProperty.Should().Be(5);
            CustomJsonConfig.Current.StringProperty.Should().Be("str2");
            CustomJsonConfig.Current.BoolProperty.Should().Be(true);
            CustomJsonConfig.Current.BoolProperty2.Should().Be(false);
            CustomJsonConfig.Current.StringArrayValues.Should().Equal(new[] { "str4" });
            CustomJsonConfig.Current.StringListValues.Should().Equal(new[] { "str1", "str2", "str3", "str4" });

            CustomJsonConfig.Current.Section.StringProperty.Should().Be("section_str");
            CustomJsonConfig.Current.Section.BoolProperty.Should().Be(false);

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
                },
                new CustomJsonConfig.CustomItemSection
                {
                    Name = "item3",
                    Value = 9
                }
            });
        }

        public override void TearDown()
        {
            base.TearDown();

            CustomJsonConfig.Current = null;
        }
    }
}
