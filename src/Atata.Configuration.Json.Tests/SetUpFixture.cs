using NUnit.Framework;

namespace Atata.Configuration.Json.Tests
{
    [SetUpFixture]
    public class SetUpFixture
    {
        [OneTimeSetUp]
        public void SetUp()
        {
            DriverJsonMapperAliases.Register<ChromeDriverJsonMapperOverride>(DriverJsonMapperAliases.Chrome);
        }
    }
}
