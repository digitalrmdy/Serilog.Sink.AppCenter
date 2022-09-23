# Serilog.Sink.AppCenter

AppCenter sink for Serilog

[![NuGet Badge](https://buildstats.info/nuget/serilog.sink.appcenter)](https://www.nuget.org/packages/Serilog.Sink.AppCenter/)
[![Build Status](https://app.bitrise.io/app/c70826ff4fce371e/status.svg?token=uVtXKCrVm2IaVz_MES-c1g&branch=master)](https://app.bitrise.io/app/c70826ff4fce371e)
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=digitalrmdy_Serilog.Sink.AppCenter&metric=code_smells)](https://sonarcloud.io/dashboard?id=digitalrmdy_Serilog.Sink.AppCenter)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=digitalrmdy_Serilog.Sink.AppCenter&metric=sqale_rating)](https://sonarcloud.io/dashboard?id=digitalrmdy_Serilog.Sink.AppCenter)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=digitalrmdy_Serilog.Sink.AppCenter&metric=security_rating)](https://sonarcloud.io/dashboard?id=digitalrmdy_Serilog.Sink.AppCenter)
[![develop-build](https://github.com/digitalrmdy/Serilog.Sink.AppCenter/actions/workflows/develop-build.yml/badge.svg)](https://github.com/digitalrmdy/Serilog.Sink.AppCenter/actions/workflows/develop-build.yml)
[![main-build](https://github.com/digitalrmdy/Serilog.Sink.AppCenter/actions/workflows/main-build.yml/badge.svg)](https://github.com/digitalrmdy/Serilog.Sink.AppCenter/actions/workflows/main-build.yml)
## Usage

```
Log.Logger = new LoggerConfiguration()
    .WriteTo.AppCenterSink(AppCenterTarget.ExceptionsAsCrashes, "yourAppCenterSecret")
    .CreateLogger();
```
