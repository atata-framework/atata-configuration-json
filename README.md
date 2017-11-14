# Atata.Configuration.Json

[![Docs](https://img.shields.io/badge/docs-Atata_Framework-orange.svg)](https://atata-framework.github.io/)
[![Twitter](https://img.shields.io/badge/follow-@AtataFramework-blue.svg)](https://twitter.com/AtataFramework)

C#/.NET package for JSON configuration of [Atata](https://github.com/atata-framework/atata).

## Features

* Full configutaion of `Atata` context via JSON file
* Full `WebDriver` instance configuration
* Multi-driver configuration
* Merged configuration via multiple files
* Custom configuration settings (accounts, settings)
* Multiple environments support

## Usage

Add `Atata.json` file (with "Copy to Output Directory" = "Copy if newer") to `Atata` test application project.
Use `ApplyJsonConfig` configurational extension methods to apply JSON config.

### JSON

#### Use Chrome with NUnit settings

```json
{
  "driver": {
    "type": "Chrome",
    "options": {
      "arguments": [ "disable-extensions", "start-maximized" ]
    }
  },
  "baseUrl": "https://atata-framework.github.io/atata-sample-app/#!/",
  "useNUnitTestName": true,
  "logNUnitError": true,
  "takeScreenshotOnNUnitError": true,
  "takeScreenshotOnNUnitErrorTitle": "Fail",
  "logConsumers": [
    {
      "type": "Atata.NUnitTestContextLogConsumer, Atata",
      "minLevel": "Info",
      "sectionFinish": true
    }
  ]
}
```

#### Use Firefox

```json
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

```json
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
#elif PROD
    ApplyJsonConfig(environmentAlias: "PROD"). // Applies "Atata.PROD.json" for build configuration with "PROD" conditional compilation symbol.
#endif
    Build();
```

## License

Atata is an open source software, licensed under the Apache License 2.0. See [LICENSE](LICENSE) for details.