﻿namespace Atata.Configuration.Json.Tests;

[TestFixture]
public class StandardSettingsTests : TestFixture
{
    [Test]
    public void Regular()
    {
        Subject<AtataContext> result = AtataContext.Configure()
            .ApplyJsonConfig(@"Configs/StandardSettings.json")
            .Build()
            .ToResultSubject();

        result.ValueOf(x => x.DriverInitializationStage).Should.Be(AtataContextDriverInitializationStage.OnDemand);

        result.ValueOf(x => x.BaseUrl).Should.Be("https://demo.atata.io/");

        result.ValueOf(x => x.Artifacts.FullName.Value).Should.Be(
            Path.Combine(
                AtataContext.GlobalProperties.ArtifactsRootPath,
                nameof(StandardSettingsTests),
                $"prefix_{TestContext.CurrentContext.Test.Name}_postfix"));

        result.ValueOf(x => x.Variables["customIntVar"]).Should.Be(7L);
        result.ValueOf(x => x.Variables["customStringVar"]).Should.Be("strvar");

        result.ValueOf(x => x.DomTestIdAttributeName).Should.Be("data-autoid");
        result.ValueOf(x => x.DomTestIdAttributeDefaultCase).Should.Be(TermCase.MidSentence);
    }
}
