using System;
using Serilog.Events;
using Xunit;

namespace Serilog.Sink.AppCenter.Tests
{
	public class AppCenterSinkTests
	{
		[Theory]
		[InlineData(LogEventLevel.Warning)]
		[InlineData(LogEventLevel.Information)]
		[InlineData(LogEventLevel.Error)]
		[InlineData(LogEventLevel.Fatal)]
		public void LogLevel_PropertiesContainsLevel(LogEventLevel level)
		{
			// Arrange
			var testableAppCenterSink = new TestableAppCenterSink();
			using (var logger = new LoggerConfiguration()
				.WriteTo.Sink(testableAppCenterSink)
				.CreateLogger())
			{
				// Act
				logger.Write(level, "test");
			}

			// Assert
			var val = Assert.Contains("level", testableAppCenterSink.LastAnalyticsProperties);
			Assert.Equal(level.ToString(), val);
		}

		[Theory]
		[InlineData("something")]
		[InlineData("something else")]
		public void FormattedMessage_CorrectProperties(string subject)
		{
			// Arrange
			const string logId = "logSubject";
			const string logMessage = "Successfully logged {logSubject}";

			var testableAppCenterSink = new TestableAppCenterSink();
			using (var logger = new LoggerConfiguration()
				.WriteTo.Sink(testableAppCenterSink)
				.CreateLogger())
			{
				// Act
				logger.Information(logMessage, subject);
			}

			// Assert
			var val = Assert.Contains(logId, testableAppCenterSink.LastAnalyticsProperties);
			Assert.Equal(subject, val);
			Assert.Equal(logMessage, testableAppCenterSink.LastMessage);
		}

		[Fact]
		public void Target_OnlyCrashes()
		{
			// Arrange
			var testableAppCenterSink = new TestableAppCenterSink(AppCenterTarget.ExceptionsAsCrashes);
			using (var logger = new LoggerConfiguration()
				.WriteTo.Sink(testableAppCenterSink)
				.CreateLogger())
			{
				// Act
				logger.Information(new Exception("test"), "test");
			}

			// Assert
			Assert.Null(testableAppCenterSink.LastAnalyticsProperties);
			Assert.NotNull(testableAppCenterSink.LastCrashProperties);
		}

		[Fact]
		public void Target_OnlyAnalytics()
		{
			// Arrange
			var testableAppCenterSink = new TestableAppCenterSink(AppCenterTarget.ExceptionsAsEvents);
			using (var logger = new LoggerConfiguration()
				.WriteTo.Sink(testableAppCenterSink)
				.CreateLogger())
			{
				// Act
				logger.Information(new Exception("test"), "test");
			}

			// Assert
			Assert.NotNull(testableAppCenterSink.LastAnalyticsProperties);
			Assert.Null(testableAppCenterSink.LastCrashProperties);
		}

		[Fact]
		public void Target_BothCrashesAndAnalytics()
		{
			// Arrange
			var testableAppCenterSink = new TestableAppCenterSink(AppCenterTarget.ExceptionsAsCrashesAndEvents);
			using (var logger = new LoggerConfiguration()
				.WriteTo.Sink(testableAppCenterSink)
				.CreateLogger())
			{
				// Act
				logger.Information(new Exception("test"), "test");
			}

			// Assert
			Assert.NotNull(testableAppCenterSink.LastAnalyticsProperties);
			Assert.NotNull(testableAppCenterSink.LastCrashProperties);
		}

		[Theory]
		[InlineData("example")]
		[InlineData("another example")]
		public void Crashes_IncludeMessages(string messageSample)
		{
			// Arrange
			var testableAppCenterSink = new TestableAppCenterSink();
			using (var logger = new LoggerConfiguration()
				.WriteTo.Sink(testableAppCenterSink)
				.CreateLogger())
			{
				// Act
				logger.Information(new Exception("test"), messageSample);
			}

			// Assert
			var val = Assert.Contains("message", testableAppCenterSink.LastCrashProperties);
			Assert.Equal(messageSample, val);
		}

		[Theory]
		[InlineData("example")]
		[InlineData("another example")]
		public void Crashes_IncludeExceptions(string exceptionMessage)
		{
			// Arrange
			var testableAppCenterSink = new TestableAppCenterSink();
			using (var logger = new LoggerConfiguration()
				.WriteTo.Sink(testableAppCenterSink)
				.CreateLogger())
			{
				// Act
				logger.Information(new Exception(exceptionMessage), "test");
			}

			// Assert
			Assert.NotNull(testableAppCenterSink.LastException);
			Assert.Equal(exceptionMessage, testableAppCenterSink.LastException.Message);
		}
	}
}