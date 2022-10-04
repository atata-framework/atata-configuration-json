namespace Atata.Configuration.Json.Tests;

public class RemoteDriverJsonMapperOverride : RemoteDriverJsonMapper
{
    protected override RemoteDriverAtataContextBuilder CreateDriverBuilder(AtataContextBuilder builder) =>
        builder.UseDriver(new RemoteDriverAtataContextBuilderOverride(builder.BuildingContext));
}
