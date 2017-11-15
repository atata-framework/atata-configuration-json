using System;
using System.IO;
using Atata.Configuration.Json;
using Newtonsoft.Json;

namespace Atata
{
    public static class JsonAtataContextBuilderExtensions
    {
        private const string DefaultConfigFileName = "Atata";

        private const string DefaultConfigFileExtension = ".json";

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
            string completeFilePath = BuildCompleteFilePath(filePath, environmentAlias);

            string jsonContent = File.ReadAllText(completeFilePath);

            TConfig config = JsonConvert.DeserializeObject<TConfig>(jsonContent);

            AtataContextBuilder resultBuilder = JsonConfigMapper.Map(config, builder);

            JsonConfigManager<TConfig>.UpdateCurrentValue(jsonContent, config);

            if (builder == AtataContext.GlobalConfiguration)
            {
                JsonConfigManager<TConfig>.UpdateGlobalValue(jsonContent);
            }
            else if (!resultBuilder.BuildingContext.CleanUpActions.Contains(JsonConfigManager<TConfig>.ResetCurrentValue))
            {
                resultBuilder.BuildingContext.CleanUpActions.Add(JsonConfigManager<TConfig>.ResetCurrentValue);
            }

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

        private static string BuildCompleteFilePath(string filePath, string environmentAlias)
        {
            string completeFilePath = null;
            string environmentAliasInsertion = string.IsNullOrWhiteSpace(environmentAlias) ? null : $".{environmentAlias}";

            if (string.IsNullOrWhiteSpace(filePath))
            {
                completeFilePath = $"{DefaultConfigFileName}{environmentAliasInsertion}{DefaultConfigFileExtension}";
            }
            else
            {
                if (filePath.EndsWith(Path.DirectorySeparatorChar.ToString()))
                {
                    completeFilePath = $"{filePath}{DefaultConfigFileName}{environmentAliasInsertion}{DefaultConfigFileExtension}";
                }
                else if (Path.HasExtension(filePath))
                {
                    if (environmentAliasInsertion == null)
                        completeFilePath = filePath;
                    else
                        completeFilePath = $"{Path.GetFileNameWithoutExtension(filePath)}{environmentAliasInsertion}{Path.GetExtension(filePath)}";
                }
                else
                {
                    completeFilePath = $"{filePath}{environmentAliasInsertion}{DefaultConfigFileExtension}";
                }
            }

            if (!Path.IsPathRooted(completeFilePath))
                completeFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, completeFilePath);

            return completeFilePath;
        }
    }
}
