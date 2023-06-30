# Serilog.Sink.AppCenter

AppCenter sink for Serilog

[![NuGet Badge](https://buildstats.info/nuget/serilog.sink.appcenter)](https://www.nuget.org/packages/Serilog.Sink.AppCenter/)
[![Develop](https://github.com/digitalrmdy/Serilog.Sink.AppCenter/actions/workflows/push.yml/badge.svg?branch=develop)](https://github.com/digitalrmdy/Serilog.Sink.AppCenter/actions/workflows/push.yml)
[![Publish](https://github.com/digitalrmdy/Serilog.Sink.AppCenter/actions/workflows/publish.yml/badge.svg)](https://github.com/digitalrmdy/Serilog.Sink.AppCenter/actions/workflows/publish.yml)
## Usage

```
Log.Logger = new LoggerConfiguration()
    .WriteTo.AppCenterSink(AppCenterTarget.ExceptionsAsCrashes, "yourAppCenterSecret")
    .CreateLogger();
```
