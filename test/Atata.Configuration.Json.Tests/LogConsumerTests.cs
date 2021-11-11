using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Atata.Configuration.Json.Tests
{
    [TestFixture]
    public class LogConsumerTests : TestFixture
    {
        [Test]
        public void Multiple_ViaSingleConfig()
        {
            AtataContextBuilder builder = AtataContext.Configure().
                ApplyJsonConfig("Configs/LogConsumers");

            LogConsumerInfo[] expected =
            {
                new LogConsumerInfo(new DebugLogConsumer { Separator = " - " }),
                new LogConsumerInfo(new TraceLogConsumer(), LogLevel.Trace, true)
                {
                    MessageNestingLevelIndent = "_ ",
                    MessageStartSectionPrefix = "S:",
                    MessageEndSectionPrefix = "E:",
                },
                new LogConsumerInfo(new NUnitTestContextLogConsumer(), LogLevel.Info, false),
                new LogConsumerInfo(new NLogConsumer { LoggerName = "somelogger" }, LogLevel.Warn, false),
                new LogConsumerInfo(new CustomLogConsumer { IntProperty = 15 }, LogLevel.Error)
            };

            AssertLogConsumers(expected, builder.BuildingContext.LogConsumers);

            JsonConfig.Current.LogConsumers.Count.Should().Be(expected.Length);
        }

        [Test]
        public void Multiple_ViaMultipleConfigs()
        {
            AtataContextBuilder builder = AtataContext.Configure().
                ApplyJsonConfig("Configs/DebugLogConsumers").
                ApplyJsonConfig("Configs/TraceLogConsumers");

            LogConsumerInfo[] expected =
            {
                new LogConsumerInfo(new DebugLogConsumer { Separator = " - " }),
                new LogConsumerInfo(new TraceLogConsumer(), LogLevel.Trace, true)
            };

            AssertLogConsumers(expected, builder.BuildingContext.LogConsumers);

            JsonConfig.Current.LogConsumers.Select(x => x.Type)
                .Should().Equal(LogConsumerAliases.Debug, LogConsumerAliases.Trace);
        }

        private static void AssertLogConsumers(IEnumerable<LogConsumerInfo> expected, IEnumerable<LogConsumerInfo> actual)
        {
            actual.Should().BeEquivalentTo(
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
