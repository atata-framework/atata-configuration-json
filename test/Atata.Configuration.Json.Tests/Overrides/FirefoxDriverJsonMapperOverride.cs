namespace Atata.Configuration.Json.Tests
{
    public class FirefoxDriverJsonMapperOverride : FirefoxDriverJsonMapper
    {
        protected override FirefoxAtataContextBuilder CreateDriverBuilder(AtataContextBuilder builder)
        {
            return builder.UseDriver(new FirefoxAtataContextBuilderOverride(builder.BuildingContext));
        }
    }
}
