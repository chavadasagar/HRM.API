using HRM.API.Application.Catalog;
using HRM.API.Application.Common;
using HRM.API.Application.Identity.Roles;

namespace HRM.API.Host.Controllers;

public class DropdownController : VersionedApiController
{

    private readonly IRoleService _roleService;

    public DropdownController(IRoleService roleService) => _roleService = roleService;

    [HttpGet("brands")]
    [OpenApiOperation("get brand list.", "")]
    public async Task<List<BrandDropdownDto>> GetBrandsAsync()
    {
        return await Mediator.Send(new GetActiveBrandsRequest());
    }

    [HttpGet("categories")]
    [OpenApiOperation("get category list.", "")]
    public async Task<List<CategoryDropdownDto>> GetCategoriesAsync()
    {
        return await Mediator.Send(new GetActiveCategoriesRequest());
    }

    [HttpGet("countries")]
    [OpenApiOperation("get country list.", "")]
    public async Task<List<CountryDto>> GetCountryAsync()
    {
        return await Mediator.Send(new GetCountriesRequest());
    }

    [HttpGet("states/{id:guid}")]
    [OpenApiOperation("get state list by country.", "")]
    public async Task<List<StateDto>> GetStatesByCountryAsync(Guid id)
    {
        return await Mediator.Send(new GetStatesRequest(id));
    }

    [HttpGet("roles")]
    [OpenApiOperation("Get a list of all roles.", "")]
    public Task<List<RoleDto>> GetRoleListAsync(CancellationToken cancellationToken)
    {
        return _roleService.GetListAsync(cancellationToken);
    }

}
