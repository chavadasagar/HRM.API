using System.Collections.ObjectModel;

namespace HRM.API.Shared;

public static class FSHResource
{
    public const string General = nameof(General);
}

public static class MPOSConfigurations
{
    private static readonly ConfigurationModel[] _all = new ConfigurationModel[]
    {
        new(FSHResource.General, "DateFormat",  "Date Format", "dd-mm-yyyy", 1),
        new(FSHResource.General, "TimeFormat",  "Time Format", "12 Hours", 2),
        new(FSHResource.General, "Decimals",  "Decimals", "2", 3),
        new(FSHResource.General, "Currency",  "Currency Symbol", "₹", 4),
        new(FSHResource.General, "EnableRoundOff",  "Enable Round Off", "true", 5)
    };

    public static IReadOnlyList<ConfigurationModel> All { get; } = new ReadOnlyCollection<ConfigurationModel>(_all);
}

public record ConfigurationModel(string Resource, string Key, string configText, string configValue, int sortOrder)
{
    public string ConfigKey => NameFor(Resource, Key);
    public static string NameFor(string resource, string key) => $"{resource}.{key}";

    public string ConfigText = configText;
    public string ConfigValue = configValue;
    public int SortOrder = sortOrder;
}