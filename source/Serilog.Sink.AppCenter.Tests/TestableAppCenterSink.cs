using System;
using System.Collections.Generic;

namespace Serilog.Sink.AppCenter.Tests
{
	internal class TestableAppCenterSink: AppCenterSink
	{
		public Exception LastException { get; private set; }
		public IReadOnlyDictionary<string, string> LastCrashProperties { get; private set; }
		public IReadOnlyDictionary<string, string> LastAnalyticsProperties { get; private set; }
		public string LastMessage { get; private set; }

		public TestableAppCenterSink(AppCenterTarget target = default, IFormatProvider formatProvider = null) : base(target, null, formatProvider)
		{
		}

		protected override void TrackCrash(Exception exception, Dictionary<string, string> properties)
		{
			LastException = exception;
			LastCrashProperties = properties;
		}

		protected override void TrackEvent(string message, Dictionary<string, string> properties)
		{
			LastMessage = message;
			LastAnalyticsProperties = properties;
		}
	}
}