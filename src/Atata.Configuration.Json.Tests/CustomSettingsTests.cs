using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Atata.Configuration.Json.Tests
{
    [TestFixture]
    public class CustomSettingsTests : TestFixture
    {
        [Test]
        public void Regular()
        {
            AtataContext.Configure().
                ApplyJsonConfig<CustomJsonConfig>(@"Configs/CustomSettings.json");

            CustomJsonConfig.Current.BaseUrl.Should().Be("https://demo.atata.io/");
            CustomJsonConfig.Current.IntProperty.Should().Be(5);
            CustomJsonConfig.Current.StringProperty.Should().Be("str");
            CustomJsonConfig.Current.BoolProperty.Should().Be(true);
            CustomJsonConfig.Current.BoolProperty2.Should().Be(false);
            CustomJsonConfig.Current.StringArrayValues.Should().Equal(new[] { "str1", "str2", "str3" });
            CustomJsonConfig.Current.StringListValues.Should().Equal(new[] { "str1", "str2", "str3" });

            CustomJsonConfig.Current.Section.StringProperty.Should().Be("section_str");
            CustomJsonConfig.Current.Section.BoolProperty.Should().Be(true);

            CustomJsonConfig.Current.Items.Should().BeEquivalentTo(new[]
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
        public void Merged()
        {
            AtataContext.Configure().
                ApplyJsonConfig<CustomJsonConfig>(@"Configs/CustomSettings.json").
                ApplyJsonConfig<CustomJsonConfig>(@"Configs/CustomSettingsOverride.json");

            CustomJsonConfig.Current.BaseUrl.Should().Be("https://demo.atata.io/override");
            CustomJsonConfig.Current.IntProperty.Should().Be(5);
            CustomJsonConfig.Current.StringProperty.Should().Be("str2");
            CustomJsonConfig.Current.BoolProperty.Should().Be(true);
            CustomJsonConfig.Current.BoolProperty2.Should().Be(false);
            CustomJsonConfig.Current.StringArrayValues.Should().Equal(new[] { "str4" });
            CustomJsonConfig.Current.StringListValues.Should().Equal(new[] { "str1", "str2", "str3", "str4" });

            CustomJsonConfig.Current.Section.StringProperty.Should().Be("section_str");
            CustomJsonConfig.Current.Section.BoolProperty.Should().Be(false);

            CustomJsonConfig.Current.Items.Should().BeEquivalentTo(new[]
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

            CustomJsonConfig.Current.Drivers.Should().HaveCount(1);
            CustomJsonConfig.Current.Drivers[0].Options.Arguments.Should().Equal("disable-extensions");
        }

        [Test]
        public void GlobalThenCurrent()
        {
            AtataContext.GlobalConfiguration.
                ApplyJsonConfig<CustomJsonConfig>(@"Configs/CustomSettings.json");

            AtataContext.Configure().
                ApplyJsonConfig<CustomJsonConfig>(@"Configs/CustomSettingsOverride.json").
                Build();

            CustomJsonConfig.Global.BaseUrl.Should().Be("https://demo.atata.io/");
            CustomJsonConfig.Current.BaseUrl.Should().Be("https://demo.atata.io/override");

            CustomJsonConfig.Global.StringProperty.Should().Be("str");
            CustomJsonConfig.Current.StringProperty.Should().Be("str2");

            CustomJsonConfig.Global.StringListValues.Should().Equal(new[] { "str1", "str2", "str3" });
            CustomJsonConfig.Current.StringListValues.Should().Equal(new[] { "str1", "str2", "str3", "str4" });

            AtataContext parallelAtataContext = null;
            CustomJsonConfig parallelCustomJsonConfig = null;

            Task.Run(() =>
            {
                parallelAtataContext = AtataContext.Configure().
                    ApplyJsonConfig<CustomJsonConfig>(@"Configs/CustomSettingsOverride2.json").
                    Build();

                parallelCustomJsonConfig = CustomJsonConfig.Current;
            }).Wait();

            try
            {
                CustomJsonConfig.Global.BaseUrl.Should().Be("https://demo.atata.io/");

                CustomJsonConfig.Current.BaseUrl.Should().Be("https://demo.atata.io/override");
                CustomJsonConfig.Current.StringProperty.Should().Be("str2");
                CustomJsonConfig.Current.StringListValues.Should().Equal(new[] { "str1", "str2", "str3", "str4" });

                parallelCustomJsonConfig.BaseUrl.Should().Be("https://demo.atata.io/override2");
                parallelCustomJsonConfig.StringProperty.Should().Be("str3");
                parallelCustomJsonConfig.StringListValues.Should().Equal(new[] { "str1", "str2", "str3", "str5" });

                AtataContext.Current.CleanUp();

                CustomJsonConfig.Current.Should().BeNull();

                CustomJsonConfig.Global.BaseUrl.Should().Be("https://demo.atata.io/");
                CustomJsonConfig.Global.StringProperty.Should().Be("str");
                CustomJsonConfig.Global.StringListValues.Should().Equal(new[] { "str1", "str2", "str3" });
            }
            finally
            {
                parallelAtataContext.CleanUp();
            }
        }
    }
}
