using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Atata.Configuration.Json
{
    /// <summary>
    /// Provides static methods for loading of JSON configuration from file.
    /// </summary>
    public static class JsonConfigFile
    {
        /// <summary>
        /// The default file name is "Atata".
        /// </summary>
        public const string DefaultFileName = "Atata";

        /// <summary>
        /// The default file extension is ".json".
        /// </summary>
        public const string DefaultFileExtension = ".json";

        /// <summary>
        /// Reads the JSON config file and deserializes it to an object of <see cref="JsonConfig"/> type.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="environmentAlias">The environment alias.</param>
        /// <returns>The deserialized object of <see cref="JsonConfig"/> type.</returns>
        public static JsonConfig Read(string filePath = null, string environmentAlias = null)
        {
            return Read<JsonConfig>(filePath, environmentAlias);
        }

        /// <summary>
        /// Reads the JSON config file and deserializes it to an object of <typeparamref name="TConfig"/> type.
        /// </summary>
        /// <typeparam name="TConfig">The type of the configuration class.</typeparam>
        /// <param name="filePath">The file path.</param>
        /// <param name="environmentAlias">The environment alias.</param>
        /// <returns>The deserialized object of <typeparamref name="TConfig"/> type.</returns>
        public static TConfig Read<TConfig>(string filePath = null, string environmentAlias = null)
        {
            string jsonContent = ReadText(filePath, environmentAlias);

            return JsonConvert.DeserializeObject<TConfig>(jsonContent);
        }

        /// <summary>
        /// Reads the text from the JSON config file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="environmentAlias">The environment alias.</param>
        /// <returns>A string containing content of JSON file.</returns>
        public static string ReadText(string filePath = null, string environmentAlias = null)
        {
            string fullFilePath = GetFullPath(filePath, environmentAlias);

            return File.ReadAllText(fullFilePath);
        }

        /// <summary>
        /// Reads the default "Atata.json" config file, deserializes it and returns the driver aliases.
        /// </summary>
        /// <returns>An array of the driver aliases.</returns>
        public static string[] ReadDriverAliasesFromDefaultConfig()
        {
            return ReadDriverAliases();
        }

        /// <summary>
        /// Reads the JSON config file, deserializes it and returns the driver aliases.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="environmentAlias">The environment alias.</param>
        /// <returns>An array of the driver aliases.</returns>
        public static string[] ReadDriverAliases(string filePath = null, string environmentAlias = null)
        {
            return Read<JsonConfig>(filePath, environmentAlias).Drivers?.Select(x => x.Alias).ToArray() ?? new string[0];
        }

        /// <summary>
        /// Returns the full/absolute path for the file using optionally <paramref name="filePath"/> and <paramref name="environmentAlias"/> arguments.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="environmentAlias">The environment alias.</param>
        /// <returns>The full file path.</returns>
        public static string GetFullPath(string filePath = null, string environmentAlias = null)
        {
            string fullPath = null;
            string environmentAliasInsertion = string.IsNullOrWhiteSpace(environmentAlias) ? null : $".{environmentAlias}";

            if (string.IsNullOrWhiteSpace(filePath))
            {
                fullPath = $"{DefaultFileName}{environmentAliasInsertion}{DefaultFileExtension}";
            }
            else
            {
                if (filePath.EndsWith(Path.DirectorySeparatorChar.ToString()))
                {
                    fullPath = $"{filePath}{DefaultFileName}{environmentAliasInsertion}{DefaultFileExtension}";
                }
                else if (Path.HasExtension(filePath))
                {
                    if (environmentAliasInsertion == null)
                        fullPath = filePath;
                    else
                        fullPath = $"{Path.GetFileNameWithoutExtension(filePath)}{environmentAliasInsertion}{Path.GetExtension(filePath)}";
                }
                else
                {
                    fullPath = $"{filePath}{environmentAliasInsertion}{DefaultFileExtension}";
                }
            }

            if (!Path.IsPathRooted(fullPath))
                fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fullPath);

            return fullPath;
        }
    }
}
