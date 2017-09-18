using System;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Opera;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;

namespace Atata.Configuration.Json
{
    public class RemoteDriverJsonMapper : IDriverJsonMapper
    {
        public void Map(DriverJsonSection section, AtataContextBuilder builder)
        {
            RemoteDriverAtataContextBuilder driverBuilder = CreateDriverBuilder(builder);

            Map(section, driverBuilder);
        }

        public DriverOptions CreateOptions(DriverOptionsJsonSection section)
        {
            IDriverJsonMapper mapper = GetOptionsMapper(section.Type);
            return mapper.CreateOptions(section);
        }

        protected virtual RemoteDriverAtataContextBuilder CreateDriverBuilder(AtataContextBuilder builder)
        {
            return builder.UseRemoteDriver();
        }

        protected virtual void Map(DriverJsonSection section, RemoteDriverAtataContextBuilder builder)
        {
            if (!string.IsNullOrWhiteSpace(section.Alias))
                builder.WithAlias(section.Alias);

            if (section.CommandTimeout != null)
                builder.WithCommandTimeout(TimeSpan.FromSeconds(section.CommandTimeout.Value));

            if (!string.IsNullOrWhiteSpace(section.RemoteAddress))
                builder.WithRemoteAddress(section.RemoteAddress);

            if (section.Options != null)
                builder.WithOptions(() => CreateOptions(section.Options));

            if (section.Capabilities != null)
                builder.WithCapabilities(() => CreateCapabilities(section.Capabilities));
        }

        private IDriverJsonMapper GetOptionsMapper(string typeName)
        {
            switch (typeName?.ToLower())
            {
                case "chrome":
                    return new ChromeDriverJsonMapper();
                case "firefox":
                    return new FirefoxDriverJsonMapper();
                case "internetexplorer":
                    return new InternetExplorerDriverJsonMapper();
                case "safari":
                    return new SafariDriverJsonMapper();
                case "opera":
                    return new OperaDriverJsonMapper();
                case "edge":
                    return new EdgeDriverJsonMapper();
                case "phantomjs":
                    return new PhantomJSDriverJsonMapper();
                case null:
                    throw new ArgumentNullException(nameof(typeName), "Options type name is not defined.");
                default:
                    throw new ArgumentException($"Unsupported options type name: {typeName}.", nameof(typeName));
            }
        }

        private ICapabilities CreateCapabilities(DesiredCapabilitiesJsonSection section)
        {
            DesiredCapabilities capabilities = CreateCapabilities(section.Type);

            foreach (var item in section.ExtraPropertiesMap)
                capabilities.SetCapability(item.Key, item.Value);

            if (section.Proxy != null)
                capabilities.SetCapability(CapabilityType.Proxy, section.Proxy.ToProxy());

            return capabilities;
        }

        private DesiredCapabilities CreateCapabilities(string typeName)
        {
            switch (typeName?.ToLower())
            {
                case "chrome":
                    throw CreateArgumentExceptionForIncorrectTypeName(typeName, nameof(ChromeOptions));
                case "firefox":
                    throw CreateArgumentExceptionForIncorrectTypeName(typeName, nameof(FirefoxOptions));
                case "internetexplorer":
                    throw CreateArgumentExceptionForIncorrectTypeName(typeName, nameof(InternetExplorerOptions));
                case "safari":
                    throw CreateArgumentExceptionForIncorrectTypeName(typeName, nameof(SafariOptions));
                case "opera":
                    throw CreateArgumentExceptionForIncorrectTypeName(typeName, nameof(OperaOptions));
                case "edge":
                    throw CreateArgumentExceptionForIncorrectTypeName(typeName, nameof(EdgeOptions));
                case "phantomjs":
                    return DesiredCapabilities.PhantomJS();
                case "htmlunit":
                    return DesiredCapabilities.HtmlUnit();
                case "htmlunitwithjavascript":
                    return DesiredCapabilities.HtmlUnitWithJavaScript();
                case null:
                    return new DesiredCapabilities();
                default:
                    throw CreateArgumentExceptionForIncorrectTypeName(typeName);
            }
        }

        private Exception CreateArgumentExceptionForIncorrectTypeName(string typeName, string recommendedOptionsType = null)
        {
            StringBuilder builder = new StringBuilder($"Unsupported capabilities type name: \"{typeName}\".");

            if (recommendedOptionsType != null)
                builder.Append($" Use {recommendedOptionsType} instead.");

            return new ArgumentException(builder.ToString(), nameof(typeName));
        }
    }
}
