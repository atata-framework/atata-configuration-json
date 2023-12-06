namespace Atata.Configuration.Json.Tests;

public class CommonDriverPropertiesTests : TestFixture
{
    private Subject<ChromeAtataContextBuilder> _sut;

    [SetUp]
    public void SetUpSuite()
    {
        AtataContextBuilder builder = AtataContext.Configure()
            .ApplyJsonConfig("Configs/CommonDriverProperties.json");

        _sut = ((ChromeAtataContextBuilder)builder.BuildingContext.DriverFactoryToUse).ToSutSubject();
    }

    [Test]
    public void CreateRetries() =>
        _sut.ValueOf(x => x.CreateRetries).Should.Be(7);

    [Test]
    public void InitialHealthCheck() =>
        _sut.ValueOf(x => x.InitialHealthCheck).Should.BeTrue();
}
