using OpenQA.Selenium.Firefox;

namespace Atata.Configuration.Json;

public class FirefoxDriverJsonMapper : DriverJsonMapper<FirefoxAtataContextBuilder, FirefoxDriverService, FirefoxOptions>
{
    protected override FirefoxAtataContextBuilder CreateDriverBuilder(AtataContextBuilder builder) =>
        builder.UseFirefox();

    protected override void MapOptions(DriverOptionsJsonSection section, FirefoxOptions options)
    {
        base.MapOptions(section, options);

        if (section.AdditionalBrowserOptions != null)
        {
            foreach (var item in section.AdditionalBrowserOptions.ExtraPropertiesMap)
                options.AddAdditionalFirefoxOption(item.Key, FillTemplateVariables(item.Value));
        }

        if (section.Arguments is { Length: > 0 })
            options.AddArguments(section.Arguments);

        if (section.Profile != null)
        {
            if (options.Profile == null || !string.IsNullOrWhiteSpace(section.Profile.ProfileDirectory) || section.Profile.DeleteSourceOnClean != null)
                options.Profile = CreateProfile(section.Profile);

            MapProfile(section.Profile, options.Profile);
        }

        if (section.Preferences != null)
        {
            foreach (var item in section.Preferences.ExtraPropertiesMap)
                SetOptionsPreference(item.Key, item.Value, options);
        }

        if (section.AndroidOptions != null)
            options.AndroidOptions = CreateAndMapAndroidOptions(section.AndroidOptions);
    }

    private static void SetOptionsPreference(string name, object value, FirefoxOptions options)
    {
        switch (value)
        {
            case bool castedValue:
                options.SetPreference(name, castedValue);
                break;
            case int castedValue:
                options.SetPreference(name, castedValue);
                break;
            case long castedValue:
                options.SetPreference(name, castedValue);
                break;
            case double castedValue:
                options.SetPreference(name, castedValue);
                break;
            case string castedValue:
                options.SetPreference(name, FillTemplateVariables(castedValue));
                break;
            case null:
                options.SetPreference(name, null);
                break;
            default:
                throw new ArgumentException($"Unsupported {nameof(FirefoxOptions)} preference value type: {value.GetType().FullName}. Supports: bool, int, long, double, string.", nameof(value));
        }
    }

    private static FirefoxProfile CreateProfile(DriverProfileJsonSection section)
    {
        string profileDirectory = string.IsNullOrWhiteSpace(section.ProfileDirectory)
            ? null
            : section.ProfileDirectory;

        return new FirefoxProfile(profileDirectory, section.DeleteSourceOnClean ?? false);
    }

    private static void SetProfilePreference(string name, object value, FirefoxProfile profile)
    {
        switch (value)
        {
            case bool castedValue:
                profile.SetPreference(name, castedValue);
                break;
            case int castedValue:
                profile.SetPreference(name, castedValue);
                break;
            case string castedValue:
                profile.SetPreference(name, FillTemplateVariables(castedValue));
                break;
            case null:
                throw new ArgumentNullException(nameof(value), $"Unsupported {nameof(FirefoxProfile)} preference value: null. Supports: string, int, bool.");
            default:
                throw new ArgumentException($"Unsupported {nameof(FirefoxProfile)} preference value type: {value.GetType().FullName}. Supports: bool, int, string.", nameof(value));
        }
    }

    private void MapProfile(DriverProfileJsonSection section, FirefoxProfile profile)
    {
        ObjectMapper.Map(section.ExtraPropertiesMap, profile);

        if (section.Extensions != null)
        {
            foreach (var item in section.Extensions)
                profile.AddExtension(item);
        }

        if (section.Preferences != null)
        {
            foreach (var item in section.Preferences.ExtraPropertiesMap)
                SetProfilePreference(item.Key, item.Value, profile);
        }
    }

    private FirefoxAndroidOptions CreateAndMapAndroidOptions(AndroidOptionsJsonSection section)
    {
        if (string.IsNullOrEmpty(section.AndroidPackage))
            throw new ConfigurationException(
                "\"androidPackage\" configuration property of \"androidOptions\" section is not specified.");

        var androidOptions = new FirefoxAndroidOptions(FillTemplateVariables(section.AndroidPackage));
        ObjectMapper.Map(section.ExtraPropertiesMap, androidOptions);

        if (section.AndroidIntentArguments != null)
            androidOptions.AddIntentArguments(section.AndroidIntentArguments);

        return androidOptions;
    }
}
