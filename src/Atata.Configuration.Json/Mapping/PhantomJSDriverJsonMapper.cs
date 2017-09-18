using System.Linq;
using OpenQA.Selenium.PhantomJS;

namespace Atata.Configuration.Json
{
    public class PhantomJSDriverJsonMapper : DriverJsonMapper<PhantomJSAtataContextBuilder, PhantomJSDriverService, PhantomJSOptions>
    {
        protected override PhantomJSAtataContextBuilder CreateDriverBuilder(AtataContextBuilder builder)
        {
            return builder.UsePhantomJS();
        }

        protected override void MapService(DriverServiceJsonSection section, PhantomJSDriverService service)
        {
            base.MapService(section, service);

            if (section.Arguments?.Any() ?? false)
                service.AddArguments(section.Arguments);
        }
    }
}
