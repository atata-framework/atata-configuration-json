using System;
using System.Reflection;
using Newtonsoft.Json;

namespace Atata.Configuration.Json
{
    internal static class JsonConfigManager<TConfig>
        where TConfig : JsonConfig<TConfig>
    {
        internal static void UpdateGlobalValue(string jsonContent)
        {
            PropertyInfo configProperty = GetConfigProperty(nameof(JsonConfig.Global));

            if (configProperty.GetValue(null, null) is TConfig currentConfig)
                JsonConvert.PopulateObject(jsonContent, currentConfig);
            else
                currentConfig = JsonConvert.DeserializeObject<TConfig>(jsonContent);

            configProperty.SetValue(null, currentConfig, null);
        }

        internal static void UpdateCurrentValue(string jsonContent, TConfig config)
        {
            PropertyInfo configProperty = GetConfigProperty(nameof(JsonConfig.Current));

            if (configProperty.GetValue(null, null) is TConfig currentConfig)
                JsonConvert.PopulateObject(jsonContent, currentConfig);
            else
                currentConfig = config;

            configProperty.SetValue(null, currentConfig, null);
        }

        internal static void ResetCurrentValue()
        {
            object globalValue = GetConfigProperty(nameof(JsonConfig.Global)).GetValue(null, null);

            GetConfigProperty(nameof(JsonConfig.Current)).SetValue(null, globalValue, null);
        }

        private static PropertyInfo GetConfigProperty(string name)
        {
            Type type = typeof(TConfig);
            PropertyInfo property = type.GetProperty(name, BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy);

            return property ?? throw new MissingMemberException(type.FullName, name);
        }
    }
}
