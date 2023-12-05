namespace Atata.Configuration.Json.Tests;

[TestFixture]
public class LogConsumerTests : TestFixture
{
    [Test]
    public void Multiple_ViaSingleConfig()
    {
        AtataContextBuilder builder = AtataContext.Configure()
            .ApplyJsonConfig("Configs/LogConsumers");

        LogConsumerConfiguration[] expected =
        {
            new(new DebugLogConsumer { Separator = " - " }),
            new(new TraceLogConsumer(), LogLevel.Trace)
            {
                MessageNestingLevelIndent = "_ ",
                MessageStartSectionPrefix = "S:",
                MessageEndSectionPrefix = "E:",
            },
            new(new NUnitTestContextLogConsumer(), LogLevel.Info, LogSectionEndOption.Exclude),
            new(new NLogConsumer { LoggerName = "somelogger" }, LogLevel.Warn, LogSectionEndOption.IncludeForBlocks),
            new(new CustomLogConsumer { IntProperty = 15 }, LogLevel.Error)
        };

        AssertLogConsumers(expected, builder.BuildingContext.LogConsumerConfigurations);

        JsonConfig.Current.LogConsumers.Count.Should().Be(expected.Length);
    }

    [Test]
    public void Multiple_ViaMultipleConfigs()
    {
        AtataContextBuilder builder = AtataContext.Configure()
            .ApplyJsonConfig("Configs/DebugLogConsumers")
            .ApplyJsonConfig("Configs/TraceLogConsumers");

        LogConsumerConfiguration[] expected =
        {
            new(new DebugLogConsumer { Separator = " - " }),
            new(new TraceLogConsumer(), LogLevel.Trace)
        };

        AssertLogConsumers(expected, builder.BuildingContext.LogConsumerConfigurations);

        JsonConfig.Current.LogConsumers.Select(x => x.Type)
            .Should().Equal(LogConsumerAliases.Debug, LogConsumerAliases.Trace);
    }

    private static void AssertLogConsumers(IEnumerable<LogConsumerConfiguration> expected, IEnumerable<LogConsumerConfiguration> actual) =>
        actual.Should().BeEquivalentTo(
            expected,
            opt => opt.IncludingAllRuntimeProperties().Using<ILogConsumer>(ctx =>
            {
                ctx.Subject.Should().BeOfType(ctx.Expectation.GetType());
                ctx.Subject.Should().BeEquivalentTo(ctx.Expectation, opt2 => opt2.IncludingAllRuntimeProperties());
            }).WhenTypeIs<ILogConsumer>());

    public class CustomLogConsumer : ILogConsumer
    {
        public int? IntProperty { get; set; }

        public void Log(LogEventInfo eventInfo) =>
            throw new NotSupportedException();
    }
}
