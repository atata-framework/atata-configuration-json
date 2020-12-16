# Atata.Configuration.Json

[![NuGet](http://img.shields.io/nuget/v/Atata.Configuration.Json.svg?style=flat)](https://www.nuget.org/packages/Atata.Configuration.Json/)
[![GitHub release](https://img.shields.io/github/release/atata-framework/atata-configuration-json.svg)](https://github.com/atata-framework/atata-configuration-json/releases)
[![Build status](https://dev.azure.com/atata-framework/atata-configuration-json/_apis/build/status/atata-configuration-json-ci?branchName=master)](https://dev.azure.com/atata-framework/atata-configuration-json/_build/latest?definitionId=33&branchName=master)
[![Gitter](https://badges.gitter.im/atata-framework/atata-configuration-json.svg)](https://gitter.im/atata-framework/atata-configuration-json)
[![Slack](https://img.shields.io/badge/join-Slack-green.svg?colorB=4EB898)](https://join.slack.com/t/atata-framework/shared_invite/zt-5j3lyln7-WD1ZtMDzXBhPm0yXLDBzbA)
[![Atata docs](https://img.shields.io/badge/docs-Atata_Framework-orange.svg)](https://atata.io)
[![Twitter](https://img.shields.io/badge/follow-@AtataFramework-blue.svg)](https://twitter.com/AtataFramework)

C#/.NET package for [Atata](https://github.com/atata-framework/atata) configuration through JSON files.

Supports .NET Framework 4.0+ and .NET Core/Standard 2.0+.

**[What's new in v1.5.0](https://atata.io/blog/2020/12/16/atata.configuration.json-1.5.0-released/)**

## Table of Contents

- [Features](#features)
- [Installation](#installation)
- [Usage](#usage)
  - [JSON](#json)
  - [Apply Configuration](#apply-configuration)
  - [Get Config Properties](#get-config-properties)
  - [Custom Settings](#custom-settings)
- [JSON Schema](#json-schema)
  - [Type Name Values](#type-name-values)
- [Feedback](#feedback)
- [License](#license)

## Features

* Full configuration of Atata context via JSON file
* Custom settings
* Multi-driver configuration
* Merged configuration via multiple files
* Multiple environments support

## Installation

Install [`Atata.Configuration.Json`](https://www.nuget.org/packages/Atata.Configuration.Json/) NuGet package.

- Package Manager:

  ```
  Install-Package Atata.Configuration.Json
  ```

- .NET CLI:

  ```
  dotnet add package Atata.Configuration.Json
  ```

## Usage

* Add `Atata.json` file (with property "Copy to Output Directory" = "Copy if newer") to Atata test project.
* Use one of `ApplyJsonConfig` configurational extension methods to apply JSON config.

### JSON

#### Use Chrome with NUnit settings

```js
{
  "driver": {
    "type": "chrome",
    "options": {
      "arguments": [ "start-maximized" ]
    }
  },
  "baseUrl": "https://demo.atata.io/",
  "culture": "en-US",
  "useAllNUnitFeatures": true
}
```

#### Use Firefox

```js
{
  "driver": {
    "type": "firefox",
    "options": {
      "arguments": [ "--start-maximized" ]
      }
    }
  }
}
```

#### Multi-Driver

```js
{
  "drivers": [
    {
      "type": "chrome",
      "options": {
        "arguments": [ "start-maximized" ]
      }
    },
    {
      "type": "firefox",
      "options": {
        "arguments": [ "--start-maximized" ]
      }
    }
  ]
}

```

### Apply Configuration

#### Default

```cs
AtataContext.Configure().
    ApplyJsonConfig(). // Applies default "Atata.json" config.
    Build();
```

#### Default Globally

```cs
AtataContext.GlobalConfiguration.
    ApplyJsonConfig();
```

#### Named

```cs
AtataContext.Configure().
    ApplyJsonConfig("Config.json"). // Applies "Config.json" config.
    Build();
```

#### Named With Environment

```cs
AtataContext.Configure().
    ApplyJsonConfig("Config", environmentAlias: "QA"). // Applies "Config.QA.json" config.
    Build();
```

#### Environment by Preprocessor Directive

```cs
AtataContext.Configure().
    ApplyJsonConfig(). // Applies default "Atata.json" config.
#if QA
    ApplyJsonConfig(environmentAlias: "QA"). // Applies "Atata.QA.json" for build configuration with "QA" conditional compilation symbol.
#elif STAGING
    ApplyJsonConfig(environmentAlias: "STAGING"). // Applies "Atata.STAGING.json" for build configuration with "STAGING" conditional compilation symbol.
#endif
    Build();
```

### Get Config Properties

#### Current

Use `JsonConfig.Current` to get current configuration properties.

```cs
string baseUrl = JsonConfig.Current.BaseUrl;
```

#### Global

Use `JsonConfig.Global` to get global configuration properties.

```cs
string baseUrl = JsonConfig.Global.BaseUrl;
```

### Custom Settings

#### Define Config Settings

```js
{
  // Driver, url and other standard settings...

  "intProperty": 5,
  "stringProperty": "str",
  "boolProperty": true,
  "stringListValues": [ "str1", "str2", "str3" ],
  "section": {
    "stringProperty": "section_str",
    "boolProperty": true
  },
  "items": [
    {
      "name": "item1",
      "value": 5
    },
    {
      "name": "item2",
      "value": 7
    }
  ]
}
```

#### Define Config Class

Config class should inherit `JsonConfig<TConfig>` from `Atata.Configuration.Json` namespace.

```cs
using System.Collections.Generic;
using Atata.Configuration.Json;

namespace SampleApp
{
    public class AppConfig : JsonConfig<AppConfig>
    {
        public int IntProperty { get; set; }

        public string StringProperty { get; set; }

        public bool BoolProperty { get; set; }

        public List<string> StringListValues { get; set; }

        public CustomSection Section { get; set; }

        public List<CustomItemSection> Items { get; set; }

        public class CustomSection
        {
            public string StringProperty { get; set; }

            public bool BoolProperty { get; set; }
        }

        public class CustomItemSection
        {
            public string Name { get; set; }

            public int Value { get; set; }
        }
    }
}

```

#### Apply Config

```cs
AtataContext.Configure().
    ApplyJsonConfig<AppConfig>().
    Build();
```

#### Use Config

```cs
string rootStringValue = AppConfig.Current.StringProperty;

string sectionBoolValue = AppConfig.Current.Section.BoolProperty;
```

## JSON Schema

```js
{
  "drivers": [ // Use "drivers" for multiple drivers support.
    {
      // See driver settings below.
    },
    {
    }
  ],
  "driver": { // Use "driver" for single driver support.
    "type": "chrome", // Supports: "remote", "chrome", "firefox", "internetexplorer", "safari", "opera", "edge" and custom mappers registered via DriverJsonMapperAliases.Register method.
                      // Custom RemoteWebDriver type can also be passed as a full type name, e.g.: "Namespace.Class, MyAssembly".
    "alias": "custom_alias", // Use aliases when you have several drivers of the same type.
    "remoteAddress": "http://127.0.0.1:8888/wd/hub", // Remote driver specific.
    "options": { // Configures driver options.
      "type": "chrome", // Remote driver specific.
                       // Supports: chrome, firefox, internetexplorer, safari, opera, edge.
      "loggingPreferences": { // Dictionary of logType and logLevel.
                              // Invokes SetLoggingPreference method of DriverOptions for each item.
        "browser": "Info",
        "driver": "Warning"
      },
      "additionalCapabilities": { // Dictionary of capabilityName and capabilityValue.
                                  // Invokes AddAdditionalCapability method of DriverOptions for each item.
        "cap1": true,
        "cap2": 5,
        "cap3": "str"
      },
      "globalAdditionalCapabilities": { // Chrome, Firefox, InternetExplorer and Opera specific.
                                        // Dictionary of capabilityName and capabilityValue.
                                        // Invokes AddAdditionalCapability(name, value, true) method of driver specific options (e.g., ChromeOptions) for each item.
        "globalcap1": true,
        "globalcap2": 5,
        "globalcap3": "str"
      },
      "proxy": { // Chrome, Firefox, Opera and Internet Explorer specific.
                 // Configures instance of OpenQA.Selenium.Proxy type.
        "kind": "Manual" // Supports values of OpenQA.Selenium.ProxyKind enum.
        "httpProxy": "string",
        "ftpProxy": "string",
        "sslProxy": "string",
        "socksProxy": "string",
        "socksUserName": "string",
        "socksPassword": "string",
        "proxyAutoConfigUrl": "string",
        "bypassAddresses": [ "string", "string" ]
      },
      "arguments": [ "string", "string" ], // Chrome, Firefox and Opera specific.
      "excludedArguments": [ "string", "string" ], // Chrome and Opera specific.
      "extensions": [ "string", "string" ], // Chrome, Edge and Opera specific.
      "encodedExtensions": [ "string", "string" ], // Chrome and Opera specific.
      "windowTypes": [ "string", "string" ], // Chrome specific.
      "performanceLoggingPreferences": { // Chrome specific.
                                         // Configures instance of OpenQA.Selenium.Chrome.ChromePerformanceLoggingPreferences type.
        "isCollectingNetworkEvents": false,
        "isCollectingPageEvents": false,
        "isCollectingTimelineEvents": false,
        "bufferUsageReportingInterval": "00:01:10",
        "tracingCategories": [ "string", "string" ]
      },
      "userProfilePreferences": { // Chrome and Opera specific.
                                  // Dictionary of preferenceName and preferenceValue.
                                  // Invokes AddUserProfilePreference method of driver specific options (e.g., ChromeOptions) for each item.
        "pref1": false,
        "pref2": "str"
      },
      "localStatePreferences": { // Chrome and Opera specific.
                                 // Dictionary of preferenceName and preferenceValue.
                                 // Invokes AddLocalStatePreference method of driver specific options (e.g., ChromeOptions) for each item.
        "pref1": 2.7,
        "pref2": true
      },
      "mobileEmulationDeviceName": "string", // Chrome specific.
                                             // Invokes EnableMobileEmulation(string deviceName) method of ChromeOptions.
      "mobileEmulationDeviceSettings": { // Chrome specific.
                                         // Configures instance of OpenQA.Selenium.Chrome.ChromeMobileEmulationDeviceSettings type.
                                         // Invokes EnableMobileEmulation(ChromeMobileEmulationDeviceSettings deviceSettings) method of ChromeOptions.
        "userAgent": "string",
        "width": 100,
        "height": 100,
        "pixelRation": 1.5,
        "enableTouchEvents": true
      },
      "profile": { // Firefox specific.
                   // Configures instance of OpenQA.Selenium.Firefox.FirefoxProfile type.
        "profileDirectory": "string",
        "deleteSourceOnClean": true,
        "preferences": { // Dictionary of name and value.
                         // Invokes SetPreference method of FirefoxProfile for each item.
          "pref1": true,
          "pref2": 5,
          "pref3": "str"
        },
        "extensions": [ "string", "string" ],
        "{{profilePropertyName}}": "value" // Any property of FirefoxProfile type.
      },
      "preferences": { // Firefox specific.
                       // Dictionary of preferenceName and preferenceValue.
                       // Invokes SetPreference method of FirefoxOptions for each item.
        "pref1": true,
        "pref2": 5,
        "pref3": "str"
      },
      "{{driverOptionsPropertyName}}": "value" // Any property of driver specific options (e.g.: ChromeOptions).
    },
    "service": { // Configures driver service.
      "driverPath": "string", // Sets absolute or relative driver folder path.
                              // Allows "{basedir}" value at the beginning that equals AppDomain.CurrentDomain.BaseDirectory.
      "driverExecutableFileName": "string", // Sets the name of the driver executable file.
      "{{driverServicePropertyName}}": "value" // Any property of driver specific service (e.g.: ChromeDriverService).
    },
    "commandTimeout": 60, // Sets the command timeout in seconds.
    "portsToIgnore": [ 60001, 60002 ] // Sets the ports to ignore while creating driver service.
  },
  "baseUrl": "string",
  "culture": "string", // For example: "en-US".

  "baseRetryTimeout": 5, // Sets the base retry timeout in seconds.
  "baseRetryInterval": 0.5, // Sets the base retry interval in seconds.
  "elementFindTimeout": 5, // Sets the element find timeout in seconds.
  "elementFindRetryInterval": 0.5, // Sets the element find retry interval in seconds.
  "waitingTimeout": 5, // Sets the waiting timeout in seconds.
  "waitingRetryInterval": 0.5, // Sets the waiting retry interval in seconds.
  "verificationTimeout": 5, // Sets the verification timeout in seconds.
  "verificationRetryInterval": 0.5, // Sets the verification retry interval in seconds.

  "assertionExceptionType": "string", // Replaces Atata.AssertionException type with custom type, e.g.: "NUnit.Framework.AssertionException, nunit.framework".
  "aggregateAssertionExceptionType": "string", // Replaces Atata.AggregateAssertionException type with custom type, e.g.: "MyApp.AggregateAssertionException, MyApp.Library".
  "aggregateAssertionStrategyType": "string", // Sets the type name of the aggregate assertion strategy. The type should implement IAggregateAssertionStrategy.
  "warningReportStrategyType": "string", // Sets the type name of the strategy for warning assertion reporting. The type should implement IWarningReportStrategy.

  "defaultAssemblyNamePatternToFindTypes": "regex_string",
  "assemblyNamePatternToFindComponentTypes": "regex_string",
  "assemblyNamePatternToFindAttributeTypes": "regex_string",

  "useAllNUnitFeatures": true, // Indicates to enable all NUnit features for Atata.
  // Or enable particular NUnit configuration options:
  "useNUnitTestName": true,
  "logNUnitError": true,
  "takeScreenshotOnNUnitError": true,
  "takeScreenshotOnNUnitErrorTitle": "string",
  "useNUnitAggregateAssertionStrategy": true, // Indicates to use NUnitAggregateAssertionStrategy as the aggregate assertion strategy.
  "useNUnitWarningReportStrategy": true, // Indicates to use NUnitWarningReportStrategy as the strategy for warning assertion reporting.

  "logConsumers": [ // Configures list of log consumers.
    {
      "type": "nunit", // Supports: "debug", "trace", "nunit", "nlog" and custom consumers registered via LogConsumerAliases.Register method.
                       // Custom ILogConsumer type can also be passed as a full type name, e.g.: "Namespace.Class, MyAssembly".
      "minLevel": "Info", // Supports: Trace, Debug, Info, Warn, Error, Fatal.
      "sectionFinish": true,
      "messageNestingLevelIndent": "- ",
      "messageStartSectionPrefix": "> ",
      "messageEndSectionPrefix": "< ",
      "{{logConsumerPropertyName}}": "value" // Any property of log consumer.
    }
  ],
  "screenshotConsumers": [ // Configures list of screenshot consumers.
    {
      "type": "file", // Supports: "file" and custom consumers registered via ScreenshotConsumerAliases.Register method.
                      // Custom IScreenshotConsumer type can also be passed as a full type name, e.g.: "Namespace.Class, MyAssembly".
      "{{screenshotConsumerPropertyName}}": "value" // Any property of screenshot consumer, e.g.: "filePath", "fileName", "folderPath", "imageFormat".
    }
  ],

  "attributes": { // Configures context attributes.
    "global": [
      {
        "type": "attribute type",
        "{{attributeValueName}}": "value" // Any property or constructor parameter of attribute.
      }
    ],
    "assembly": [
      {
        "name": "assembly name",
        "attributes": [
          {
            "type": "attribute type",
            "{{attributeValueName}}": "value" // Any property or constructor parameter of attribute.
          }
        ]
      }
    ],
    "component": [
      {
        "type": "component type",
        "attributes": [
          {
            "type": "attribute type",
            "{{attributeValueName}}": "value" // Any property or constructor parameter of attribute.
          }
        ],
        "properties": [
          {
            "name": "property name",
            "attributes": [
              {
                "type": "attribute type",
                "{{attributeValueName}}": "value" // Any property or constructor parameter of attribute.
              }
            ]
          }
        ]
      }
    ]
  }
}
```

### Type Name Values

There are a few ways to specify a type name, for example, of attribute or component:

- Full assembly-qualified name: `Product.Lib.SomeClass, Product.Lib`
- Namespace-qualified name: `Product.Lib.SomeClass`
- Partial namespace-qualified name: `Lib.SomeClass`
- Just type name: `SomeClass`

Find out more information on [AssemblyQualifiedName](https://docs.microsoft.com/en-us/dotnet/api/system.type.assemblyqualifiedname) docs,
which also contains information about a format of generic and nested type names.

## Feedback

Any feedback, issues and feature requests are welcome.

If you faced an issue please report it to [Atata.Configuration.Json Issues](https://github.com/atata-framework/atata-configuration-json/issues),
write to [Atata.Configuration.Json Gitter](https://gitter.im/atata-framework/atata-configuration-json)
or just mail to yevgeniy.shunevych@gmail.com.

## License

Atata is an open source software, licensed under the Apache License 2.0.
See [LICENSE](LICENSE) for details.
