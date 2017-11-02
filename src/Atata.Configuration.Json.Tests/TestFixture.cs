using NUnit.Framework;

namespace Atata.Configuration.Json.Tests
{
    public class TestFixture
    {
        [TearDown]
        public virtual void TearDown()
        {
            JsonConfig.Current = null;
            CustomJsonConfig.Current = null;
        }
    }
}
