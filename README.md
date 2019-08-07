# Serilog.Sink.AppCenter

AppCenter sink for Serilog

[![Build Status](https://app.bitrise.io/app/c70826ff4fce371e/status.svg?token=uVtXKCrVm2IaVz_MES-c1g&branch=master)](https://app.bitrise.io/app/c70826ff4fce371e)
[![CodeFactor](https://www.codefactor.io/repository/github/3factr/serilog.sink.appcenter/badge)](https://www.codefactor.io/repository/github/3factr/serilog.sink.appcenter)
[![NuGet Badge](https://buildstats.info/nuget/serilog.sink.appcenter)](https://www.nuget.org/packages/Serilog.Sink.AppCenter/)

## Usage

```
Log.Logger = new LoggerConfiguration()
    .WriteTo.AppCenterSink(AppCenterTarget.ExceptionsAsCrashes, "yourAppCenterSecret")
    .CreateLogger();
```
