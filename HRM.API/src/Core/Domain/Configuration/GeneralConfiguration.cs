namespace HRM.API.Domain.Configuration;
public class GeneralConfiguration : AuditableEntity, IAggregateRoot
{
    public string ConfigKey { get; set; } = default!;
    public string ConfigText { get; set; } = default!;
    public string ConfigValue { get; set; } = default!;
    public int SortOrder { get; set; }

    public GeneralConfiguration(string configKey, string configText, string configValue, int sortOrder)
    {
        ConfigKey = configKey;
        ConfigText = configText;
        ConfigValue = configValue;
        SortOrder = sortOrder;
    }
}
