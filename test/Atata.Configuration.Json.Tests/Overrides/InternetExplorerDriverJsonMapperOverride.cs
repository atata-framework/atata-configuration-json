namespace Atata.Configuration.Json.Tests;

public class InternetExplorerDriverJsonMapperOverride : InternetExplorerDriverJsonMapper
{
    protected override InternetExplorerAtataContextBuilder CreateDriverBuilder(AtataContextBuilder builder) =>
        builder.UseDriver(new InternetExplorerAtataContextBuilderOverride(builder.BuildingContext));
}
