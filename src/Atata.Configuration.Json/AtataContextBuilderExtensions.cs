using System;
using System.IO;
using Newtonsoft.Json;

namespace Atata
{
    public static class AtataContextBuilderJsonExtensions
    {
        public static AtataContextBuilder ApplyJsonConfig(this AtataContextBuilder builder, string filePath)
        {
            return builder.ApplyJsonConfig<JsonConfig>(filePath);
        }

        public static AtataContextBuilder ApplyJsonConfig<TConfig>(this AtataContextBuilder builder, string filePath)
            where TConfig : JsonConfig<TConfig>
        {
            if (!Path.IsPathRooted(filePath))
                filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath);

            string jsonContent = File.ReadAllText(filePath);

            TConfig config = JsonConvert.DeserializeObject<TConfig>(jsonContent);

            // TODO: Apply configuration properties.
            return builder;
        }
    }
}
