using FluentAssertions;
using NUnit.Framework;

namespace Atata.Configuration.Json.Tests
{
    public class AttributeTests : TestFixture
    {
        [Test]
        public void Global()
        {
            AtataContextBuilder builder = AtataContext.Configure().
                ApplyJsonConfig("Configs/Attributes/Global");

            var result = builder.BuildingContext.Attributes.Global;

            result.Should().HaveCount(4);

            var attribute1 = result[0].Should().BeOfType<TermFindSettingsAttribute>().Subject;
            attribute1.Case.Should().Be(TermCase.LowerMerged);
            attribute1.TargetTypes.Should().Equal(typeof(Field<,>));
            attribute1.TargetAttributeTypes.Should().Equal(typeof(FindByIdAttribute));

            var attribute2 = result[1].Should().BeOfType<FindSettingsAttribute>().Subject;
            attribute2.Visibility.Should().Be(Visibility.Any);
            attribute2.TargetTypes.Should().Equal(typeof(Table<,>), typeof(Table<,,>));
            attribute2.TargetAttributeTypes.Should().Equal(typeof(FindByClassAttribute), typeof(FindByFieldSetAttribute), typeof(FindByLabelAttribute));

            var attribute3 = result[2].Should().BeOfType<FindByIdAttribute>().Subject;
            attribute3.Values.Should().Equal("some-id");

            var attribute4 = result[3].Should().BeOfType<FindByNameAttribute>().Subject;
            attribute4.Values.Should().Equal("name1", "name2");
        }

        [Test]
        public void Assembly()
        {
            AtataContextBuilder builder = AtataContext.Configure().
                ApplyJsonConfig("Configs/Attributes/Assembly");

            var result = builder.BuildingContext.Attributes.AssemblyMap;

            result.Should().HaveCount(2);

            var assembly1 = System.Reflection.Assembly.GetAssembly(GetType());

            result[assembly1].Should().ContainSingle()
                .Which.Should().BeOfType<FindByIdAttribute>()
                .Which.Values.Should().Equal("some-id");

            var assembly2 = System.Reflection.Assembly.GetAssembly(typeof(AtataContext));

            result[assembly2].Should().ContainSingle()
                .Which.Should().BeOfType<FindByNameAttribute>()
                .Which.Values.Should().Equal("some-name");
        }

        [Test]
        public void Component()
        {
            AtataContextBuilder builder = AtataContext.Configure().
                ApplyJsonConfig("Configs/Attributes/Component");

            var result = builder.BuildingContext.Attributes.ComponentMap;

            result.Should().HaveCount(1);

            result[typeof(OrdinaryPage)].Should().ContainSingle()
                .Which.Should().BeOfType<UrlAttribute>()
                .Which.Url.Should().Be("/some-url");
        }

        [Test]
        public void Property()
        {
            AtataContextBuilder builder = AtataContext.Configure().
                ApplyJsonConfig("Configs/Attributes/Property");

            var result = builder.BuildingContext.Attributes.PropertyMap;

            result.Should().HaveCount(2);

            result[new TypePropertyNamePair(typeof(OrdinaryPage), "Prop1")].Should().ContainSingle()
                .Which.Should().BeOfType<FindByIdAttribute>()
                .Which.Values.Should().Equal("some-id");

            result[new TypePropertyNamePair(typeof(OrdinaryPage), "Prop2")].Should().ContainSingle()
                .Which.Should().BeOfType<FindByNameAttribute>()
                .Which.Values.Should().Equal("some-name");
        }
    }
}
