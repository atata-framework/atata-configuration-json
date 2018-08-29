using Atata.Configuration.Json;
using Newtonsoft.Json;

namespace Atata
{
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
        public static AtataContextBuilder ApplyJsonConfig(this AtataContextBuilder builder, string filePath = null, string environmentAlias = null)
        {
            return builder.ApplyJsonConfig<JsonConfig>(filePath, environmentAlias);
        }

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

            if (builder == AtataContext.GlobalConfiguration)
            {
                JsonConfigManager<TConfig>.UpdateGlobalValue(jsonContent, config);

                if (!resultBuilder.BuildingContext.OnBuildingActions.Contains(JsonConfigManager<TConfig>.InitCurrentValue))
                    resultBuilder.BuildingContext.OnBuildingActions.Add(JsonConfigManager<TConfig>.InitCurrentValue);
            }
            else
            {
                JsonConfigManager<TConfig>.InitCurrentValue();
                JsonConfigManager<TConfig>.UpdateCurrentValue(jsonContent, config);
            }

            if (!resultBuilder.BuildingContext.CleanUpActions.Contains(JsonConfigManager<TConfig>.ResetCurrentValue))
                resultBuilder.BuildingContext.CleanUpActions.Add(JsonConfigManager<TConfig>.ResetCurrentValue);

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
            where TConfig : JsonConfig<TConfig>
        {
            return JsonConfigMapper.Map((TConfig)config, builder);
        }
    }
}
