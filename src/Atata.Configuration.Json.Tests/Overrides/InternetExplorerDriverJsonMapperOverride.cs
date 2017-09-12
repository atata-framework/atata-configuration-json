namespace Atata.Configuration.Json.Tests
{
    public class InternetExplorerDriverJsonMapperOverride : InternetExplorerDriverJsonMapper
    {
        protected override InternetExplorerAtataContextBuilder CreateDriverBuilder(AtataContextBuilder builder)
        {
            return builder.UseDriver(new InternetExplorerAtataContextBuilderOverride(builder.BuildingContext));
        }
    }
}
