# Atata.Configuration.Json

[![NuGet](http://img.shields.io/nuget/v/Atata.Configuration.Json.svg?style=flat)](https://www.nuget.org/packages/Atata.Configuration.Json/)
[![GitHub release](https://img.shields.io/github/release/atata-framework/atata-configuration-json.svg)](https://github.com/atata-framework/atata-configuration-json/releases)
[![Build status](https://dev.azure.com/atata-framework/atata-configuration-json/_apis/build/status/atata-configuration-json-ci?branchName=main)](https://dev.azure.com/atata-framework/atata-configuration-json/_build/latest?definitionId=33&branchName=main)
[![Slack](https://img.shields.io/badge/join-Slack-green.svg?colorB=4EB898)](https://join.slack.com/t/atata-framework/shared_invite/zt-5j3lyln7-WD1ZtMDzXBhPm0yXLDBzbA)
[![Atata docs](https://img.shields.io/badge/docs-Atata_Framework-orange.svg)](https://atata.io)
[![Twitter](https://img.shields.io/badge/follow-@AtataFramework-blue.svg)](https://twitter.com/AtataFramework)

C#/.NET package for [Atata](https://github.com/atata-framework/atata) configuration through JSON files.

*The package targets .NET Standard 2.0, which supports .NET 5+, .NET Framework 4.6.1+ and .NET Core/Standard 2.0+.*

**[What's new in v2.7.0](https://atata.io/blog/2024/01/11/atata.configuration.json-2.7.0-released/)**

## Table of Contents

- [Features](#features)
- [Installation](#installation)
- [Usage](#usage)
  - [JSON](#json)
  - [Apply Configuration](#apply-configuration)
  - [Get Config Properties](#get-config-properties)
  - [Custom Settings](#custom-settings)
  - [Reference Environment Variables](#reference-environment-variables)
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
AtataContext.Configure()
    .ApplyJsonConfig() // Applies default "Atata.json" config.
    .Build();
```

#### Default Globally

```cs
AtataContext.GlobalConfiguration
    .ApplyJsonConfig();
```

#### Named

```cs
AtataContext.Configure()
    .ApplyJsonConfig("Config.json") // Applies "Config.json" config.
    .Build();
```

#### Named With Environment

```cs
AtataContext.Configure()
    .ApplyJsonConfig("Config", environmentAlias: "QA") // Applies "Config.QA.json" config.
    .Build();
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
AtataContext.Configure()
    .ApplyJsonConfig<AppConfig>()
    .Build();
```

#### Use Config

```cs
string rootStringValue = AppConfig.Current.StringProperty;

string sectionBoolValue = AppConfig.Current.Section.BoolProperty;
```

### Reference Environment Variables

It is possible to use environment variables as configuration values of standard or custom properties
with help of `{env:VarName}` template insertions.

```js
{
  "baseUrl": "{env:BaseUrl}",
  "accountEmail": "{env:AccountEmail}",
  "accountPassword": "{env:AccountPassword}"
}
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
    "type": "chrome", // Supports: "remote", "chrome", "firefox", "internetexplorer", "safari", "edge" and custom mappers registered via DriverJsonMapperAliases.Register method.
                      // Custom RemoteWebDriver type can also be passed as a full type name, e.g.: "Namespace.Class, MyAssembly".
    "alias": "custom_alias", // Use aliases when you have several drivers of the same type.
    "createRetries": 2, // Sets the count of possible driver creation retries in case exceptions occur during creation.
    "initialHealthCheck": false, // Sets a value indicating whether to execute an initial health check. Defaults to false.
    "remoteAddress": "http://127.0.0.1:8888/wd/hub", // Remote driver specific.
    "options": { // Configures driver options.
      "type": "chrome", // Remote driver specific.
                       // Supports: "chrome", "firefox", "internetexplorer", "safari", "edge".
      "loggingPreferences": { // Dictionary of logType and logLevel.
                              // Invokes SetLoggingPreference method of DriverOptions for each item.
        "browser": "Info",
        "driver": "Warning"
      },
      "additionalOptions": { // Dictionary of additional driver options.
        "globalcap1": true,
        "globalcap2": 5,
        "globalcap3": "str"
      },
      "additionalBrowserOptions": { // Chrome, Firefox, Edge and InternetExplorer specific.
                                    // Dictionary of additional browser options.
        "cap1": true,
        "cap2": 5,
        "cap3": "str"
      },
      "proxy": { // Configures instance of OpenQA.Selenium.Proxy type.
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
      "arguments": [ "string", "string" ], // Chrome, Firefox and Edge specific.
      "excludedArguments": [ "string", "string" ], // Chrome and Edge specific.
      "extensions": [ "string", "string" ], // Chrome and Edge specific.
      "encodedExtensions": [ "string", "string" ], // Chrome and Edge specific.
      "windowTypes": [ "string", "string" ], // Chrome and Edge specific.
      "performanceLoggingPreferences": { // Chrome and Edge specific.
                                         // Configures instance of OpenQA.Selenium.Chromium.ChromiumPerformanceLoggingPreferences type.
        "isCollectingNetworkEvents": false,
        "isCollectingPageEvents": false,
        "bufferUsageReportingInterval": "00:01:10",
        "tracingCategories": [ "string", "string" ]
      },
      "userProfilePreferences": { // Chrome and Edge specific.
                                  // Dictionary of preferenceName and preferenceValue.
                                  // Invokes AddUserProfilePreference method of driver specific options (e.g., ChromeOptions) for each item.
        "pref1": false,
        "pref2": "str"
      },
      "localStatePreferences": { // Chrome and Edge specific.
                                 // Dictionary of preferenceName and preferenceValue.
                                 // Invokes AddLocalStatePreference method of driver specific options (e.g., ChromeOptions) for each item.
        "pref1": 2.7,
        "pref2": true
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
        "extensions": [ "string", "string" ]
      },
      "preferences": { // Firefox specific.
                       // Dictionary of preferenceName and preferenceValue.
                       // Invokes SetPreference method of FirefoxOptions for each item.
        "pref1": true,
        "pref2": 5,
        "pref3": "str"
      },
      "mobileEmulationDeviceName": "string", // Chrome and Edge specific.
                                             // Invokes EnableMobileEmulation(string deviceName) method of ChromeOptions.
      "mobileEmulationDeviceSettings": { // Chrome and Edge specific.
                                         // Configures instance of OpenQA.Selenium.Chromium.ChromiumMobileEmulationDeviceSettings type.
                                         // Invokes EnableMobileEmulation(ChromiumMobileEmulationDeviceSettings deviceSettings) method of ChromeOptions.
        "userAgent": "string",
        "width": 100,
        "height": 100,
        "pixelRation": 1.5,
        "enableTouchEvents": true
      },
      "androidOptions": { // Chrome, Firefox and Edge specific.
        "androidPackage": "pack1",
        "androidDeviceSerial": "serial",
        "androidActivity": "act1",
        "useRunningApp": true, // Chrome specific.
        "androidProcess": "proc", // Chrome specific.
        "androidIntentArguments": [ "arg1", "arg2" ] // Firefox specific.
      },
      "{{driverOptionsPropertyName}}": "value" // Any property of driver specific options (e.g.: ChromeOptions).
    },
    "service": { // Configures driver service.
      "driverPath": "string", // Sets absolute or relative driver directory path.
                              // Allows "{basedir}" value at the beginning that equals AppDomain.CurrentDomain.BaseDirectory.
      "driverExecutableFileName": "string", // Sets the name of the driver executable file.
      "{{driverServicePropertyName}}": "value" // Any property of driver specific service (e.g.: ChromeDriverService).
    },
    "commandTimeout": 60, // Sets the command timeout in seconds.
    "portsToIgnore": [ 60001, 60002 ] // Sets the ports to ignore while creating driver service.
  },
  "driverInitializationStage": "string", // Supports: "none", "build" and "onDemand".

  "baseUrl": "string",
  "culture": "string", // For example: "en-US".
  "timeZone": "string", // For example: "UTC".
  "artifactsPath": "string",

  "defaultControlVisibility": "string", // Supports: "any" (default), "visible" and "invisible".

  "baseRetryTimeout": 5, // Sets the base retry timeout in seconds.
  "baseRetryInterval": 0.5, // Sets the base retry interval in seconds.
  "elementFindTimeout": 5, // Sets the element find timeout in seconds.
  "elementFindRetryInterval": 0.5, // Sets the element find retry interval in seconds.
  "waitingTimeout": 5, // Sets the waiting timeout in seconds.
  "waitingRetryInterval": 0.5, // Sets the waiting retry interval in seconds.
  "verificationTimeout": 5, // Sets the verification timeout in seconds.
  "verificationRetryInterval": 0.5, // Sets the verification retry interval in seconds.

  "variables": {
    "{{anyVariableName}}": "value" // Any custom variable. Value can be string, number or boolean.
  },

  "assertionExceptionType": "string", // Replaces Atata.AssertionException type with custom type, e.g.: "NUnit.Framework.AssertionException, nunit.framework".
  "aggregateAssertionExceptionType": "string", // Replaces Atata.AggregateAssertionException type with custom type, e.g.: "MyApp.AggregateAssertionException, MyApp.Library".
  "aggregateAssertionStrategyType": "string", // Sets the type name of the aggregate assertion strategy. The type should implement IAggregateAssertionStrategy.
  "warningReportStrategyType": "string", // Sets the type name of the strategy for warning assertion reporting. The type should implement IWarningReportStrategy.
  "assertionFailureReportStrategyType": "string", // Sets the type name of the strategy for assertion failure reporting. The type should implement IAssertionFailureReportStrategy.

  "domTestIdAttributeName": "data-testid",
  "domTestIdAttributeDefaultCase": "kebab",

  "defaultAssemblyNamePatternToFindTypes": "regex_string",
  "assemblyNamePatternToFindComponentTypes": "regex_string",
  "assemblyNamePatternToFindAttributeTypes": "regex_string",
  "assemblyNamePatternToFindEventTypes": "regex_string",
  "assemblyNamePatternToFindEventHandlerTypes": "regex_string",

  "useAllNUnitFeatures": true, // Indicates to enable all Atata features for NUnit.
  "useSpecFlowNUnitFeatures": true, // Indicates to enable all Atata features for SpecFlow+NUnit.
  // Or enable particular NUnit configuration options:
  "useNUnitTestName": true,
  "useNUnitTestSuiteName": true,
  "useNUnitTestSuiteType": true,
  "onCleanUpAddDirectoryFilesToNUnitTestContext": "string",
  "useNUnitAggregateAssertionStrategy": true, // Indicates to use NUnitAggregateAssertionStrategy as the aggregate assertion strategy.
  "useNUnitWarningReportStrategy": true, // Indicates to use NUnitWarningReportStrategy as the strategy for warning assertion reporting.
  "useNUnitAssertionFailureReportStrategy": true, // Indicates to use NUnitAssertionFailureReportStrategy as the strategy for assertion failure reporting.

  "logConsumers": [ // Configures list of log consumers.
    {
      "type": "nunit", // Supports: "debug", "trace", "console", "nunit", "nlog", "nlog-file", "log4net"
                       // and custom consumers registered via LogConsumerAliases.Register method.
                       // Custom ILogConsumer type can also be passed as a full type name, e.g.: "Namespace.Class, MyAssembly".
      "minLevel": "info", // Supports: "trace", "debug", "info", "warn", "error", "fatal".
      "sectionEnd": "include", // Supports: "include", "includeForBlocks", "exclude".
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
      "{{screenshotConsumerPropertyName}}": "value" // Any property of screenshot consumer, e.g.: "filePath", "fileName", "directoryPath".
    }
  ],

  "screenshots": { // Configures screenshots functionality.
    "strategy": {
      "type": "webDriverViewport", // Supports: "webDriverViewport", "webDriverFullPage", "cdpFullPage", "fullPageOrViewport",
                                   // and name of custom type implementing "Atata.IScreenshotTakeStrategy".
      "{{strategyValueName}}": "value" // Any property or constructor parameter of strategy.
    }
  },

  "pageSnapshots": { // Configures page snapshots functionality.
    "fileNameTemplate": "string",
    "strategy": {
      "type": "cdpOrPageSource", // Supports: "cdpOrPageSource", "pageSource", "cdp",
                                 // and name of custom type implementing "Atata.IPageSnapshotTakeStrategy".
      "{{strategyValueName}}": "value" // Any property or constructor parameter of strategy.
    }
  },

  "browserLogs": { // Configures browser logs monitoring, which isn't enabled by default.
    "log": true,
    "minLevelOfWarning": "warn" // Supports: "trace", "debug", "info", "warn", "error", "fatal".
  },

  "eventSubscriptions": [
    {
      "eventType": "event type", // Optional.
      "handlerType": "handler type", // Required.
      "{{handlerValueName}}": "value" // Any property or constructor parameter of event handler.
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
