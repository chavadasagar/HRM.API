namespace MasterPOS.API.Application.Configuration;
public class ConfigurationUpdateRequest
{
    public Guid Id { get; set; }
    public string ConfigValue { get; set; } = default!;
}
