namespace Atata.Configuration.Json.Tests
{
    public class ChromeDriverJsonMapperOverride : ChromeDriverJsonMapper
    {
        protected override ChromeAtataContextBuilder CreateDriverBuilder(AtataContextBuilder builder)
        {
            return new ChromeAtataContextBuilderOverride(builder.BuildingContext);
        }
    }
}
