namespace HRM.API.Application.Configuration;
public class UpdateBulkConfigurationRequest
{
    public List<ConfigurationUpdateRequest> configurations { get; set; } = default!;
}