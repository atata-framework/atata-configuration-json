using OpenQA.Selenium;

namespace Atata.Configuration.Json
{
    public interface IDriverJsonMapper
    {
        void Map(DriverJsonSection section, AtataContextBuilder builder);

        DriverOptions CreateOptions(DriverOptionsJsonSection section);
    }
}
