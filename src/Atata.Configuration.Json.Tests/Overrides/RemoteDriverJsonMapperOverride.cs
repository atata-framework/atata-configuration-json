namespace Atata.Configuration.Json.Tests
{
    public class RemoteDriverJsonMapperOverride : RemoteDriverJsonMapper
    {
        protected override RemoteDriverAtataContextBuilder CreateDriverBuilder(AtataContextBuilder builder)
        {
            return new RemoteDriverAtataContextBuilderOverride(builder.BuildingContext);
        }
    }
}
