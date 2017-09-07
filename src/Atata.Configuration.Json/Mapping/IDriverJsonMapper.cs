using OpenQA.Selenium;

namespace Atata
{
    public interface IDriverJsonMapper
    {
        void Map(DriverJsonSection section, AtataContextBuilder builder);

        DriverOptions CreateOptions(DriverOptionsJsonSection section);
    }
}
