using System.Linq;
using OpenQA.Selenium;

namespace Atata
{
    public class ProxyJsonSection : JsonSection
    {
        public ProxyKind? Kind { get; set; }

        public string HttpProxy { get; set; }

        public string FtpProxy { get; set; }

        public string SslProxy { get; set; }

        public string SocksProxy { get; set; }

        public string SocksUserName { get; set; }

        public string SocksPassword { get; set; }

        public string ProxyAutoConfigUrl { get; set; }

        public string[] BypassAddresses { get; set; }

        public Proxy ToProxy()
        {
            Proxy proxy = new Proxy();

            if (Kind != null)
                proxy.Kind = Kind.Value;

            if (!string.IsNullOrWhiteSpace(HttpProxy))
                proxy.HttpProxy = HttpProxy;

            if (!string.IsNullOrWhiteSpace(FtpProxy))
                proxy.FtpProxy = FtpProxy;

            if (!string.IsNullOrWhiteSpace(SslProxy))
                proxy.SslProxy = SslProxy;

            if (!string.IsNullOrWhiteSpace(SocksProxy))
                proxy.SocksProxy = SocksProxy;

            if (!string.IsNullOrWhiteSpace(SocksUserName))
                proxy.SocksUserName = SocksUserName;

            if (!string.IsNullOrWhiteSpace(SocksPassword))
                proxy.SocksPassword = SocksPassword;

            if (!string.IsNullOrWhiteSpace(ProxyAutoConfigUrl))
                proxy.ProxyAutoConfigUrl = ProxyAutoConfigUrl;

            if (BypassAddresses?.Any() ?? false)
                proxy.AddBypassAddresses(BypassAddresses);

            return proxy;
        }
    }
}
