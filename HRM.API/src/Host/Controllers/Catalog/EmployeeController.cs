using HRM.API.Application.Catalog.Employees;
using HRM.API.Application.Common.Wrapper;
using Microsoft.Extensions.Localization;

namespace HRM.API.Host.Controllers.Catalog;
public class EmployeeController : VersionedApiController
{
    private readonly IStringLocalizer<EmployeeController> _localizer;
    public EmployeeController(IStringLocalizer<EmployeeController> localizer)
    {
        _localizer = localizer;
    }

    [HttpPost("create")]
    [MustHavePermission(FSHAction.Create, FSHResource.Employee)]
    [OpenApiOperation("Create a new employee.", "")]
    public async Task<ActionResult> CreateAsync(CreateEmployeeRequest request)
    {
        Guid id = await Mediator.Send(request);
        return Ok(await Result<Guid>.SuccessAsync(id, string.Format(_localizer["product.created"])));
    }

    [HttpPost("genrat-dummy-employee")]
    [MustHavePermission(FSHAction.View, FSHResource.Employee)]
    [OpenApiOperation("Genrate Dummy employee", "")]
    public async Task<ActionResult> GenrateAsync(GenrateDummyEmployeeRequest request)
    {
        var result = await Mediator.Send(request);
        return Ok(result);
    }


}
