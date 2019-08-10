using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Serilog.Core;
using Serilog.Events;

namespace Serilog.Sink.AppCenter
{
    public class AppCenterSink : ILogEventSink
    {
        protected AppCenterTarget Target { get; }
        private readonly IFormatProvider _formatProvider;

        public AppCenterSink(AppCenterTarget target = default, string appSecret = null, IFormatProvider formatProvider = null)
        {
            Target = target;
            _formatProvider = formatProvider;

            if (appSecret != null && !Microsoft.AppCenter.AppCenter.Configured)
            {
                Microsoft.AppCenter.AppCenter.Configure(appSecret);
            }

            Task.Run(EnableAppCenterTargets);
        }

        private async Task EnableAppCenterTargets()
        {
            if (!await Microsoft.AppCenter.Analytics.Analytics.IsEnabledAsync().ConfigureAwait(false))
            {
                await Microsoft.AppCenter.Analytics.Analytics.SetEnabledAsync(true).ConfigureAwait(false);
            }
            if ((Target == AppCenterTarget.ExceptionsAsCrashes || Target == AppCenterTarget.ExceptionsAsCrashesAndEvents)
                && !await Microsoft.AppCenter.Crashes.Crashes.IsEnabledAsync().ConfigureAwait(false))
            {
                await Microsoft.AppCenter.Crashes.Crashes.SetEnabledAsync(true).ConfigureAwait(false);
            }
        }

        public void Emit(LogEvent logEvent)
        {
            if (logEvent.Exception != null && Target == AppCenterTarget.ExceptionsAsCrashes || Target == AppCenterTarget.ExceptionsAsCrashesAndEvents)
            {
                TrackCrash(logEvent.Exception, ConvertToProperties(logEvent, true));

                if (Target == AppCenterTarget.ExceptionsAsCrashes)
                {
                    return;
                }
            }
  
            TrackEvent(logEvent.MessageTemplate.Text, ConvertToProperties(logEvent, false));
        }

        protected virtual void TrackEvent(string message, Dictionary<string, string> properties)
        {
            Microsoft.AppCenter.Analytics.Analytics.TrackEvent(message, properties);
        }

        protected virtual void TrackCrash(Exception exception, Dictionary<string, string> properties)
        {
            Microsoft.AppCenter.Crashes.Crashes.TrackError(exception, properties);
        }

        private Dictionary<string, string> ConvertToProperties(LogEvent logEvent, bool includeMessage)
        {
            var properties = new Dictionary<string, string>
			{
				{ "level", logEvent.Level.ToString() }
			};
            if (includeMessage)
            {
                properties.Add("message", logEvent.MessageTemplate.Text);
            }

            Parallel.ForEach(logEvent.Properties, property =>
            {
                using (var stringWriter = new StringWriter())
                {
                    property.Value.Render(stringWriter, "l", _formatProvider);
                    var str = stringWriter.ToString();
                    properties.Add(property.Key, str);
                }
            });
            return properties;
        }
    }
}
