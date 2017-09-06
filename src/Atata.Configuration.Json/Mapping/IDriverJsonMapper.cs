namespace Atata
{
    public interface IDriverJsonMapper
    {
        void Map(DriverJsonSection section, AtataContextBuilder builder);
    }
}
