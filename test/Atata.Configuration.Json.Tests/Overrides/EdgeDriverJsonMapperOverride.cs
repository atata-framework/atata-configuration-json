namespace Atata.Configuration.Json.Tests;

public class EdgeDriverJsonMapperOverride : EdgeDriverJsonMapper
{
    protected override EdgeAtataContextBuilder CreateDriverBuilder(AtataContextBuilder builder) =>
        builder.UseDriver(new EdgeAtataContextBuilderOverride(builder.BuildingContext));
}
