﻿namespace Atata.Configuration.Json.Tests;

[TestFixture]
public class TestFixture
{
    [TearDown]
    public virtual void TearDown()
    {
        AtataContext.Current?.Dispose();

        JsonConfig.Current = null;
        JsonConfig.Global = null;
        CustomJsonConfig.Current = null;
        CustomJsonConfig.Global = null;

        AtataContext.GlobalConfiguration.Clear();
    }
}
