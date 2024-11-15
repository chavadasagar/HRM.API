using MasterPOS.API.Application.Common.Wrapper;

namespace MasterPOS.API.Application.Identity.Roles;

public interface IRoleService : ITransientService
{
    Task<PaginationResponse<RoleDto>> SearchAsync(RoleListFilter filter, CancellationToken cancellationToken);

    Task<List<RoleDto>> GetListAsync(CancellationToken cancellationToken);

    Task<int> GetCountAsync(CancellationToken cancellationToken);

    Task<bool> ExistsAsync(string roleName, string? excludeId);

    Task<RoleDto> GetByIdAsync(string id);

    Task<RoleDto> GetByIdWithPermissionsAsync(string roleId, CancellationToken cancellationToken);

    Task<IResult<RoleDto>> CreateOrUpdateAsync(CreateOrUpdateRoleRequest request);

    Task<string> UpdatePermissionsAsync(UpdateRolePermissionsRequest request, CancellationToken cancellationToken);

    Task<IResult<string>> DeleteAsync(string id);
}