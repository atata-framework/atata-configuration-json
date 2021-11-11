namespace Atata.Configuration.Json.Tests
{
    public class ChromeDriverJsonMapperOverride : ChromeDriverJsonMapper
    {
        protected override ChromeAtataContextBuilder CreateDriverBuilder(AtataContextBuilder builder)
        {
            return builder.UseDriver(new ChromeAtataContextBuilderOverride(builder.BuildingContext));
        }
    }
}
