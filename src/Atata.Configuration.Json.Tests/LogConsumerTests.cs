using System;
using FluentAssertions;
using NUnit.Framework;

namespace Atata.Configuration.Json.Tests
{
    [TestFixture]
    public class LogConsumerTests : TestFixture
    {
        [Test]
        public void LogConsumer_AllKinds()
        {
            AtataContextBuilder builder = AtataContext.Configure().
                ApplyJsonConfig("LogConsumers");

            LogConsumerInfo[] expected =
            {
                new LogConsumerInfo(new DebugLogConsumer { Separator = " - " }),
                new LogConsumerInfo(new TraceLogConsumer(), LogLevel.Trace, true),
                new LogConsumerInfo(new NUnitTestContextLogConsumer(), LogLevel.Info, false),
                new LogConsumerInfo(new NLogConsumer { LoggerName = "somelogger" }, LogLevel.Warn, false),
                new LogConsumerInfo(new CustomLogConsumer { IntProperty = 15 }, LogLevel.Error)
            };

            builder.BuildingContext.LogConsumers.Should().BeEquivalentTo(
                expected,
                opt => opt.IncludingAllRuntimeProperties().Using<ILogConsumer>(ctx =>
                    {
                        ctx.Subject.Should().BeOfType(ctx.Expectation.GetType());
                        ctx.Subject.Should().BeEquivalentTo(ctx.Expectation, opt2 => opt2.IncludingAllRuntimeProperties());
                    }).WhenTypeIs<ILogConsumer>());
        }

        public class CustomLogConsumer : ILogConsumer
        {
            public int? IntProperty { get; set; }

            public void Log(LogEventInfo eventInfo)
            {
                throw new NotSupportedException();
            }
        }
    }
}
