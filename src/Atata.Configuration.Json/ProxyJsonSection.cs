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
    }
}
