using OpenQA.Selenium;

namespace Atata.Configuration.Json.Tests;

public class DriverContext<TService, TOptions>
    where TService : DriverService
{
    public bool ReturnsNull { get; set; }

    public TService Service { get; private set; }

    public TOptions Options { get; private set; }

    public TimeSpan CommandTimeout { get; private set; }

    public void Set(TService service, TOptions options, TimeSpan commandTimeout)
    {
        Service = service;
        Options = options;
        CommandTimeout = commandTimeout;
    }

    public IDisposable UseNullDriver()
    {
        ReturnsNull = true;
        return new DriverContextNullableSession(this);
    }

    private sealed class DriverContextNullableSession : IDisposable
    {
        private readonly DriverContext<TService, TOptions> _context;

        public DriverContextNullableSession(DriverContext<TService, TOptions> context) =>
            _context = context;

        public void Dispose()
        {
            _context.ReturnsNull = false;
            _context.Service?.Dispose();
        }
    }
}
