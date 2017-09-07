namespace Atata.Configuration.Json.Tests
{
    public class InternetExplorerDriverJsonMapperOverride : InternetExplorerDriverJsonMapper
    {
        protected override InternetExplorerAtataContextBuilder CreateDriverBuilder(AtataContextBuilder builder)
        {
            return new InternetExplorerAtataContextBuilderOverride(builder.BuildingContext);
        }
    }
}
