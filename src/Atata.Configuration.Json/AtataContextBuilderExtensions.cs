using System;
using System.IO;
using Newtonsoft.Json;

namespace Atata
{
    public static class AtataContextBuilderJsonExtensions
    {
        private const string DefaultConfigFileName = "Atata";

        private const string DefaultConfigFileExtension = ".json";

        public static AtataContextBuilder ApplyJsonConfig(this AtataContextBuilder builder, string filePath = null, string environmentAlias = null)
        {
            return builder.ApplyJsonConfig<JsonConfig>(filePath, environmentAlias);
        }

        public static AtataContextBuilder ApplyJsonConfig<TConfig>(this AtataContextBuilder builder, string filePath = null, string environmentAlias = null)
            where TConfig : JsonConfig<TConfig>
        {
            string completeFilePath = BuildCompleteFilePath(filePath, environmentAlias);

            string jsonContent = File.ReadAllText(completeFilePath);

            TConfig config = JsonConvert.DeserializeObject<TConfig>(jsonContent);

            // TODO: Apply configuration properties.
            return builder.ApplyJsonConfig(config);
        }

        private static AtataContextBuilder ApplyJsonConfig<TConfig>(this AtataContextBuilder builder, TConfig config)
            where TConfig : JsonConfig<TConfig>
        {
            if (config.BaseUrl != null)
                builder.UseBaseUrl(config.BaseUrl);

            return builder;
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
                bool isFilePathToFolder = filePath.EndsWith(Path.DirectorySeparatorChar.ToString());
                bool hasExtension = Path.HasExtension(filePath);

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
