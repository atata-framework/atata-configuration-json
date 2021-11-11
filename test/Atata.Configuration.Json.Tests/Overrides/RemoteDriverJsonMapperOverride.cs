namespace Atata.Configuration.Json.Tests
{
    public class RemoteDriverJsonMapperOverride : RemoteDriverJsonMapper
    {
        protected override RemoteDriverAtataContextBuilder CreateDriverBuilder(AtataContextBuilder builder)
        {
            return builder.UseDriver(new RemoteDriverAtataContextBuilderOverride(builder.BuildingContext));
        }
    }
}
