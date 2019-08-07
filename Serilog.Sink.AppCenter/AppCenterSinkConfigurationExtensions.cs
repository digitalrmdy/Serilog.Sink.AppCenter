using System;
using Serilog.Configuration;

namespace Serilog.Sink.AppCenter
{
    public static class AppCenterSinkConfigurationExtensions
    {
        public static LoggerConfiguration AppCenterSink(
              this LoggerSinkConfiguration loggerConfiguration,
              AppCenterTarget target = default,
              string appCenterSecret = null,
              IFormatProvider formatProvider = null)
        {
            return loggerConfiguration.Sink(new AppCenterSink(target, appCenterSecret, formatProvider));
        }
    }
}
