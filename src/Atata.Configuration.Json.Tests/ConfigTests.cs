using NUnit.Framework;

namespace Atata.Configuration.Json.Tests
{
    [TestFixture]
    public class ConfigTests
    {
        [Test]
        public void ChromeAndNUnit()
        {
            AtataContextBuilder builder = AtataContext.Build().
                ApplyJsonConfig("Chrome+NUnit.json");
        }
    }
}
