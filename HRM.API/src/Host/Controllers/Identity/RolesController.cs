using HRM.API.Application.Common.Wrapper;
using HRM.API.Application.Identity.Roles;

namespace HRM.API.Host.Controllers.Identity;

public class RolesController : VersionNeutralApiController
{
    private readonly IRoleService _roleService;

    public RolesController(IRoleService roleService) => _roleService = roleService;

    [HttpPost("search")]
    [MustHavePermission(FSHAction.View, FSHResource.Roles)]
    [OpenApiOperation("Search roles using available filters.", "")]
    public async Task<PaginationResponse<RoleDto>> SearchAsync(RoleListFilter request, CancellationToken cancellationToken)
    {
        return await _roleService.SearchAsync(request, cancellationToken);
    }

    [HttpGet]
    [MustHavePermission(FSHAction.View, FSHResource.Roles)]
    [OpenApiOperation("Get a list of all roles.", "")]
    public Task<List<RoleDto>> GetListAsync(CancellationToken cancellationToken)
    {
        return _roleService.GetListAsync(cancellationToken);
    }

    [HttpGet("{id}")]
    [MustHavePermission(FSHAction.View, FSHResource.Roles)]
    [OpenApiOperation("Get role details.", "")]
    public Task<RoleDto> GetByIdAsync(string id)
    {
        return _roleService.GetByIdAsync(id);
    }

    [HttpGet("{id}/permissions")]
    [MustHavePermission(FSHAction.View, FSHResource.Roles)]
    [OpenApiOperation("Get role details with its permissions.", "")]
    public Task<RoleDto> GetByIdWithPermissionsAsync(string id, CancellationToken cancellationToken)
    {
        return _roleService.GetByIdWithPermissionsAsync(id, cancellationToken);
    }

    [HttpGet("permissions")]
    [MustHavePermission(FSHAction.View, FSHResource.Roles)]
    [OpenApiOperation("Get all permissions.", "")]
    public IReadOnlyList<FSHPermission> GetPermissionsAsync()
    {
        IReadOnlyList<FSHPermission> permissions = FSHPermissions.All.
                                                Where(x => x.Resource != FSHResource.Tenants
                                                        && x.Resource != FSHResource.Hangfire
                                                        && x.Resource != FSHResource.RoleClaims
                                                        && x.Resource != FSHResource.UserRoles).ToList();
        return permissions;
    }

    [HttpPut("{id}/permissions")]
    [MustHavePermission(FSHAction.Update, FSHResource.Roles)]
    [OpenApiOperation("Update a role's permissions.", "")]
    public async Task<ActionResult<string>> UpdatePermissionsAsync(string id, UpdateRolePermissionsRequest request, CancellationToken cancellationToken)
    {
        if (id != request.RoleId)
        {
            return BadRequest();
        }

        string result = await _roleService.UpdatePermissionsAsync(request, cancellationToken);

        return Ok(await Result<string>.SuccessAsync(result));
    }

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Roles)]
    [OpenApiOperation("Create or update a role.", "")]
    public async Task<ActionResult> RegisterRoleAsync(CreateOrUpdateRoleRequest request)
    {
        return Ok(await _roleService.CreateOrUpdateAsync(request));
    }

    [HttpDelete("{id}")]
    [MustHavePermission(FSHAction.Delete, FSHResource.Roles)]
    [OpenApiOperation("Delete a role.", "")]
    public async Task<ActionResult> DeleteAsync(string id)
    {
        return Ok(await _roleService.DeleteAsync(id));
    }
}