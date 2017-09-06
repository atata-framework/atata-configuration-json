namespace Atata.Configuration.Json.Tests
{
    public class FirefoxDriverJsonMapperOverride : FirefoxDriverJsonMapper
    {
        protected override FirefoxAtataContextBuilder CreateDriverBuilder(AtataContextBuilder builder)
        {
            return new FirefoxAtataContextBuilderOverride(builder.BuildingContext);
        }
    }
}
