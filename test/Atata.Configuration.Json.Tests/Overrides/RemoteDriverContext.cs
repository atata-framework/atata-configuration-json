namespace Atata.Configuration.Json.Tests;

public class RemoteDriverContext
{
    public bool ReturnsNull { get; set; }

    public Uri RemoteAddress { get; private set; }

    public ICapabilities Capabilities { get; private set; }

    public TimeSpan CommandTimeout { get; private set; }

    public void Set(Uri remoteAddress, ICapabilities capabilities, TimeSpan commandTimeout)
    {
        RemoteAddress = remoteAddress;
        Capabilities = capabilities;
        CommandTimeout = commandTimeout;
    }

    public IDisposable UseNullDriver()
    {
        ReturnsNull = true;
        return new DriverContextNullableSession(this);
    }

    private class DriverContextNullableSession : IDisposable
    {
        private readonly RemoteDriverContext _context;

        public DriverContextNullableSession(RemoteDriverContext context) =>
            _context = context;

        public void Dispose() =>
            _context.ReturnsNull = false;
    }
}
