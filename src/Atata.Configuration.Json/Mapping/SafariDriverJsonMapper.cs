﻿using OpenQA.Selenium.Safari;

namespace Atata.Configuration.Json;

public class SafariDriverJsonMapper : DriverJsonMapper<SafariAtataContextBuilder, SafariDriverService, SafariOptions>
{
    protected override SafariAtataContextBuilder CreateDriverBuilder(AtataContextBuilder builder) =>
        builder.UseSafari();
}
