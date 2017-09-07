namespace Atata.Configuration.Json.Tests
{
    public class PhantomJSDriverJsonMapperOverride : PhantomJSDriverJsonMapper
    {
        protected override PhantomJSAtataContextBuilder CreateDriverBuilder(AtataContextBuilder builder)
        {
            return new PhantomJSAtataContextBuilderOverride(builder.BuildingContext);
        }
    }
}
