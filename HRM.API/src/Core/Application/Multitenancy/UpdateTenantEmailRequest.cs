namespace HRM.API.Application.Multitenancy;

public class UpdateTenantEmailRequest : IRequest<bool>
{
    public string Id { get; set; } = default!;
    public string AdminEmail { get; set; } = default!;

    public UpdateTenantEmailRequest(string id, string email)
    {
        Id = id;
        AdminEmail = email;
    }
}

public class UpdateTenantEmailRequestHandler : IRequestHandler<UpdateTenantEmailRequest, bool>
{
    private readonly ITenantService _tenantService;

    public UpdateTenantEmailRequestHandler(ITenantService tenantService) => _tenantService = tenantService;

    public async Task<bool> Handle(UpdateTenantEmailRequest request, CancellationToken cancellationToken)
    {
        return await _tenantService.UpdateEmailAsync(request.AdminEmail);
    }
}
