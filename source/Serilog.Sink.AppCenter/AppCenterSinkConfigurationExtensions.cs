using System;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;

namespace Serilog.Sink.AppCenter
{
	public static class AppCenterSinkConfigurationExtensions
	{
		public static LoggerConfiguration AppCenterSink(this LoggerSinkConfiguration loggerConfiguration,
			LoggingLevelSwitch levelSwitch = null, LogEventLevel logEventLevel = LogEventLevel.Verbose,
			AppCenterTarget target = default,
			string appCenterSecret = null,
			IFormatProvider formatProvider = null)
		{
			if (loggerConfiguration == null)
			{
				throw new ArgumentNullException(nameof(loggerConfiguration));
			}

			return loggerConfiguration.Sink(new AppCenterSink(target, appCenterSecret, formatProvider), logEventLevel, levelSwitch);
		}
	}
}