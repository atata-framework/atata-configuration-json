using OpenQA.Selenium.Safari;

namespace Atata
{
    public class SafariDriverJsonMapper : DriverJsonMapper<SafariAtataContextBuilder, SafariDriverService, SafariOptions>
    {
        protected override SafariAtataContextBuilder CreateDriverBuilder(AtataContextBuilder builder)
        {
            return builder.UseSafari();
        }
    }
}
