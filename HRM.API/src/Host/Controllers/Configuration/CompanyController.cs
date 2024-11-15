using HRM.API.Application.Common.Wrapper;
using HRM.API.Application.Configuration;
using HRM.API.Application.Multitenancy;
using HRM.API.Infrastructure.Multitenancy;
using Microsoft.Extensions.Localization;

namespace HRM.API.Host.Controllers.Company;

public class CompanyController : VersionNeutralApiController
{
    private readonly IStringLocalizer<CompanyController> _localizer;
    private readonly FSHTenantInfo? _currentTenant;

    public CompanyController(IStringLocalizer<CompanyController> localizer, FSHTenantInfo? currentTenant) =>
        (_localizer, _currentTenant) = (localizer, currentTenant);

    [HttpGet]
    [OpenApiOperation("get Company list.", "")]
    public async Task<CompanyDto> GetCompanyAsync()
    {
        return await Mediator.Send(new GetCompanyRequest());
    }

    [HttpPut]
    [MustHavePermission(FSHAction.Update, FSHResource.Company)]
    [OpenApiOperation("Update Companys.", "")]
    public async Task<ActionResult> UpdateAsync(UpdateCompanyRequest request)
    {
        if (string.IsNullOrWhiteSpace(_currentTenant?.Id))
        {
            return BadRequest();
        }

        string id = _currentTenant.Id;
        bool isUpdatedEmail = await Mediator.Send(new UpdateTenantEmailRequest(id, request.Email));

        if (isUpdatedEmail == false)
        {
            return BadRequest();
        }

        await Mediator.Send(request);

        return Ok(await Result.SuccessAsync(_localizer["company.updated"]));
    }
}
