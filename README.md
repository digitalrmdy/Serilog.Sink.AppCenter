# Serilog.Sink.AppCenter

AppCenter sink for Serilog

[![NuGet Badge](https://buildstats.info/nuget/serilog.sink.appcenter)](https://www.nuget.org/packages/Serilog.Sink.AppCenter/)
[![develop-build](https://github.com/digitalrmdy/Serilog.Sink.AppCenter/actions/workflows/develop-build.yml/badge.svg)](https://github.com/digitalrmdy/Serilog.Sink.AppCenter/actions/workflows/develop-build.yml)
[![main-build](https://github.com/digitalrmdy/Serilog.Sink.AppCenter/actions/workflows/main-build.yml/badge.svg)](https://github.com/digitalrmdy/Serilog.Sink.AppCenter/actions/workflows/main-build.yml)
## Usage

```
Log.Logger = new LoggerConfiguration()
    .WriteTo.AppCenterSink(AppCenterTarget.ExceptionsAsCrashes, "yourAppCenterSecret")
    .CreateLogger();
```
