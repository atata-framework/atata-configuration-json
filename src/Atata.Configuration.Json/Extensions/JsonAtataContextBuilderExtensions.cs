using Atata.Configuration.Json;

namespace Atata;

/// <summary>
/// Provides a set of extension methods for <see cref="AtataContextBuilder"/> configuration through JSON config files.
/// </summary>
public static class JsonAtataContextBuilderExtensions
{
    /// <summary>
    /// Applies JSON configuration from the file. By default reads "Atata.json" file.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="filePath">The file path.</param>
    /// <param name="environmentAlias">The environment alias.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public static AtataContextBuilder ApplyJsonConfig(this AtataContextBuilder builder, string filePath = null, string environmentAlias = null) =>
        builder.ApplyJsonConfig<JsonConfig>(filePath, environmentAlias);

    /// <summary>
    /// Applies JSON configuration from the file. By default reads "Atata.json" file.
    /// </summary>
    /// <typeparam name="TConfig">The type of the configuration class.</typeparam>
    /// <param name="builder">The builder.</param>
    /// <param name="filePath">The file path.</param>
    /// <param name="environmentAlias">The environment alias.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public static AtataContextBuilder ApplyJsonConfig<TConfig>(this AtataContextBuilder builder, string filePath = null, string environmentAlias = null)
        where TConfig : JsonConfig<TConfig>, new()
    {
        string jsonContent = JsonConfigFile.ReadText(filePath, environmentAlias);

        TConfig config = JsonConvert.DeserializeObject<TConfig>(jsonContent);

        AtataContextBuilder resultBuilder = JsonConfigMapper.Map(config, builder);

        if (builder.BuildingContext == AtataContext.GlobalConfiguration.BuildingContext)
        {
            JsonConfigManager<TConfig>.UpdateGlobalValue(jsonContent, config);

            EnsureInitConfigEventHandlerIsSubscribed<TConfig>(resultBuilder);
        }
        else
        {
            JsonConfigManager<TConfig>.InitCurrentValue();
            JsonConfigManager<TConfig>.UpdateCurrentValue(jsonContent, config);
        }

        EnsureResetConfigEventHandlerIsSubscribed<TConfig>(resultBuilder);

        resultBuilder.EventSubscriptions.Add(
            new LogJsonConfigPathEventHandler(JsonConfigFile.GetRelativePath(filePath, environmentAlias)));

        return resultBuilder;
    }

    /// <summary>
    /// Applies JSON configuration from <paramref name="config"/> parameter.
    /// </summary>
    /// <typeparam name="TConfig">The type of the configuration class.</typeparam>
    /// <param name="builder">The builder.</param>
    /// <param name="config">The configuration.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public static AtataContextBuilder ApplyJsonConfig<TConfig>(this AtataContextBuilder builder, JsonConfig<TConfig> config)
        where TConfig : JsonConfig<TConfig> =>
        JsonConfigMapper.Map((TConfig)config, builder);

    private static void EnsureInitConfigEventHandlerIsSubscribed<TConfig>(AtataContextBuilder builder)
        where TConfig : JsonConfig<TConfig>
    {
        bool isInitConfigSubscribed = builder.BuildingContext.EventSubscriptions
            .Exists(x => x.EventType == typeof(AtataContextInitStartedEvent) && x.EventHandler is InitCurrentJsonConfigEventHandler<TConfig>);

        if (!isInitConfigSubscribed)
            builder.EventSubscriptions.Add(new InitCurrentJsonConfigEventHandler<TConfig>());
    }

    private static void EnsureResetConfigEventHandlerIsSubscribed<TConfig>(AtataContextBuilder builder)
        where TConfig : JsonConfig<TConfig>
    {
        bool isResetConfigSubscribed = builder.BuildingContext.EventSubscriptions
            .Exists(x => x.EventType == typeof(AtataContextCleanUpEvent) && x.EventHandler is ResetCurrentJsonConfigEventHandler<TConfig>);

        if (!isResetConfigSubscribed)
            builder.EventSubscriptions.Add(new ResetCurrentJsonConfigEventHandler<TConfig>());
    }
}
