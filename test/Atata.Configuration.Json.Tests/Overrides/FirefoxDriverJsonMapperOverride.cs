namespace Atata.Configuration.Json.Tests;

public class FirefoxDriverJsonMapperOverride : FirefoxDriverJsonMapper
{
    protected override FirefoxAtataContextBuilder CreateDriverBuilder(AtataContextBuilder builder) =>
        builder.UseDriver(new FirefoxAtataContextBuilderOverride(builder.BuildingContext));
}
