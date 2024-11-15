using MasterPOS.API.Application.Common.Wrapper;
using MasterPOS.API.Application.Configuration;
using Microsoft.Extensions.Localization;

namespace MasterPOS.API.Host.Controllers.Configuration;

public class ConfigurationController : VersionNeutralApiController
{
    private readonly IStringLocalizer<ConfigurationController> _localizer;

    public ConfigurationController(IStringLocalizer<ConfigurationController> localizer) => _localizer = localizer;

    [HttpGet]
    [OpenApiOperation("get configuration list.", "")]
    public async Task<List<ConfigurationDto>> GetCountryAsync()
    {
        return await Mediator.Send(new GetConfigurationRequest());
    }

    [HttpPut]
    [MustHavePermission(FSHAction.Update, FSHResource.Configuration)]
    [OpenApiOperation("Update configurations.", "")]
    public async Task<ActionResult> UpdateAsync(UpdateBulkConfigurationRequest request)
    {
        if(request.configurations == null || request.configurations.Count == 0)
            return BadRequest();

        foreach (var li in request.configurations)
        {
             await Mediator.Send(new UpdateConfigurationRequest { ConfigValue = li.ConfigValue, Id = li.Id });
        }

        return Ok(await Result.SuccessAsync(_localizer["configuration.updated"]));
    }
}
