# Serilog.Sink.AppCenter

AppCenter sink for Serilog

[![NuGet Badge](https://buildstats.info/nuget/serilog.sink.appcenter)](https://www.nuget.org/packages/Serilog.Sink.AppCenter/)
[![Build Status](https://app.bitrise.io/app/c70826ff4fce371e/status.svg?token=uVtXKCrVm2IaVz_MES-c1g&branch=master)](https://app.bitrise.io/app/c70826ff4fce371e)
[![CodeFactor](https://www.codefactor.io/repository/github/3factr/serilog.sink.appcenter/badge)](https://www.codefactor.io/repository/github/3factr/serilog.sink.appcenter)
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=3factr_Serilog.Sink.AppCenter&metric=code_smells)](https://sonarcloud.io/dashboard?id=3factr_Serilog.Sink.AppCenter)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=3factr_Serilog.Sink.AppCenter&metric=coverage)](https://sonarcloud.io/dashboard?id=3factr_Serilog.Sink.AppCenter)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=3factr_Serilog.Sink.AppCenter&metric=sqale_rating)](https://sonarcloud.io/dashboard?id=3factr_Serilog.Sink.AppCenter)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=3factr_Serilog.Sink.AppCenter&metric=security_rating)](https://sonarcloud.io/dashboard?id=3factr_Serilog.Sink.AppCenter)
[![Dependabot Status](https://api.dependabot.com/badges/status?host=github&repo=3factr/Serilog.Sink.AppCenter)](https://dependabot.com)

## Usage

```
Log.Logger = new LoggerConfiguration()
    .WriteTo.AppCenterSink(AppCenterTarget.ExceptionsAsCrashes, "yourAppCenterSecret")
    .CreateLogger();
```
