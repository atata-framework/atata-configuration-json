using System;
using System.Reflection;
using Newtonsoft.Json;

namespace Atata.Configuration.Json;

internal static class JsonConfigManager<TConfig>
    where TConfig : JsonConfig<TConfig>
{
    private static readonly JsonSerializerSettings s_serializerSettings = new()
    {
        ContractResolver = JsonConfigContractResolver.Instance,
        NullValueHandling = NullValueHandling.Ignore
    };

    internal static void UpdateGlobalValue(string jsonContent, TConfig config)
    {
        PropertyInfo globalConfigProperty = GetConfigProperty(nameof(JsonConfig.Global));

        if (globalConfigProperty.GetValue(null, null) is TConfig currentConfig)
            JsonConvert.PopulateObject(jsonContent, currentConfig);
        else
            globalConfigProperty.SetValue(null, config, null);
    }

    internal static void UpdateCurrentValue(string jsonContent, TConfig config)
    {
        PropertyInfo currentConfigProperty = GetConfigProperty(nameof(JsonConfig.Current));

        if (currentConfigProperty.GetValue(null, null) is TConfig currentConfig)
            JsonConvert.PopulateObject(jsonContent, currentConfig);
        else
            currentConfigProperty.SetValue(null, config, null);
    }

    internal static void InitCurrentValue()
    {
        PropertyInfo currentConfigProperty = GetConfigProperty(nameof(JsonConfig.Current));

        if (currentConfigProperty.GetValue(null, null) == null)
        {
            object globalValue = GetConfigProperty(nameof(JsonConfig.Global)).GetValue(null, null);

            if (globalValue != null)
            {
                string serializedGlobalValue = JsonConvert.SerializeObject(globalValue, s_serializerSettings);

                object clonedGlobalValue = JsonConvert.DeserializeObject(serializedGlobalValue, globalValue.GetType());
                currentConfigProperty.SetValue(null, clonedGlobalValue, null);
            }
        }
    }

    internal static void ResetCurrentValue() =>
        GetConfigProperty(nameof(JsonConfig.Current)).SetValue(null, null, null);

    private static PropertyInfo GetConfigProperty(string name)
    {
        Type type = typeof(TConfig);
        PropertyInfo property = type.GetProperty(
            name,
            BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy);

        return property ?? throw new MissingMemberException(type.FullName, name);
    }
}
