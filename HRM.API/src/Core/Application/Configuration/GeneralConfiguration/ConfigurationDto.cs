namespace MasterPOS.API.Application.Configuration;
public class ConfigurationDto : IDto
{
    public Guid Id { get; set; }
    public string ConfigKey { get; set; } = default!;
    public string ConfigValue { get; set; } = default!;
    public string ConfigText { get; set; } = default!;
}
