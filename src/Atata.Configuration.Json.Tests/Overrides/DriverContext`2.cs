using System;

namespace Atata.Configuration.Json.Tests
{
    public class DriverContext<TService, TOptions>
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

        private class DriverContextNullableSession : IDisposable
        {
            private readonly DriverContext<TService, TOptions> context;

            public DriverContextNullableSession(DriverContext<TService, TOptions> context)
            {
                this.context = context;
            }

            public void Dispose()
            {
                context.ReturnsNull = false;
            }
        }
    }
}
