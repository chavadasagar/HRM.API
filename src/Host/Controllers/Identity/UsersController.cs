using HRM.API.Application.Common.Wrapper;
using HRM.API.Application.Identity.Users;
using HRM.API.Application.Identity.Users.Password;
using Microsoft.Extensions.Localization;
using System.Security.Claims;

namespace HRM.API.Host.Controllers.Identity;

public class UsersController : VersionNeutralApiController
{
    private readonly IUserService _userService;
    private readonly IStringLocalizer<UsersController> _localizer;
    private readonly IConfiguration _configuration;

    public UsersController(IUserService userService, IStringLocalizer<UsersController> localizer, IConfiguration configuration) =>
        (_userService, _localizer, _configuration) = (userService, localizer, configuration);

    [HttpPost("search")]
    [MustHavePermission(FSHAction.View, FSHResource.Users)]
    [OpenApiOperation("Search users using available filters.", "")]
    public async Task<PaginationResponse<UserDetailsDto>> SearchAsync(UserListFilter filter, CancellationToken cancellationToken)
    {
        if (User.GetUserId() is not { } userId || string.IsNullOrEmpty(userId))
        {
            return null;
        }

        return await _userService.SearchAsync(userId, filter, cancellationToken);
    }

    [HttpGet]
    [MustHavePermission(FSHAction.View, FSHResource.Users)]
    [OpenApiOperation("Get list of all users.", "")]
    public Task<List<UserDetailsDto>> GetListAsync(CancellationToken cancellationToken)
    {
        return _userService.GetListAsync(cancellationToken);
    }

    [HttpGet("{id}")]
    [MustHavePermission(FSHAction.View, FSHResource.Users)]
    [OpenApiOperation("Get a user's details.", "")]
    public Task<UserDetailsDto> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        return _userService.GetAsync(id, cancellationToken);
    }

    [HttpGet("{id}/roles")]
    [MustHavePermission(FSHAction.View, FSHResource.UserRoles)]
    [OpenApiOperation("Get a user's roles.", "")]
    public Task<List<UserRoleDto>> GetRolesAsync(string id, CancellationToken cancellationToken)
    {
        return _userService.GetRolesAsync(id, cancellationToken);
    }

    [HttpPost("{id}/roles")]
    [ApiConventionMethod(typeof(FSHApiConventions), nameof(FSHApiConventions.Register))]
    [MustHavePermission(FSHAction.Update, FSHResource.UserRoles)]
    [OpenApiOperation("Update a user's assigned roles.", "")]
    public Task<string> AssignRolesAsync(string id, UserRolesRequest request, CancellationToken cancellationToken)
    {
        return _userService.AssignRolesAsync(id, request, cancellationToken);
    }

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Users)]
    [OpenApiOperation("Creates a new user.", "")]
    public async Task<ActionResult> CreateAsync(CreateUserRequest request)
    {
        // TODO: check if registering anonymous users is actually allowed (should probably be an appsetting)
        // and return UnAuthorized when it isn't
        // Also: add other protection to prevent automatic posting (captcha?)
        //string referer = Request.Headers["Referer"].ToString();
        //if (string.IsNullOrEmpty(referer))
        //    referer = GetOriginFromRequest();
        string result = await _userService.CreateAsync(request, GetOriginFromRequest());

        return Ok(await Result.SuccessAsync(result));
    }

    [HttpPut]
    [MustHavePermission(FSHAction.Update, FSHResource.Users)]
    [OpenApiOperation("Update user.", "")]
    public async Task<ActionResult> UpdateAsync(UpdateUserRequest request)
    {
        string userId = request.Id;

        //string referer = Request.Headers["Referer"].ToString();
        //if (string.IsNullOrEmpty(referer))
        //    referer = GetOriginFromRequest();
        string result = await _userService.UpdateAsync(request, userId, GetOriginFromRequest());

        return Ok(await Result.SuccessAsync(result));
    }

    [HttpPut("update-status")]
    [MustHavePermission(FSHAction.UpdateStatus, FSHResource.Users)]
    [OpenApiOperation("Update user.", "")]
    public async Task<ActionResult> UpdateStatusAsync(UpdateUserStatusRequest request)
    {
        await _userService.UpdateStatusAsync(request);

        return Ok(await Result.SuccessAsync(_localizer["user.updatedstatus"]));
    }


    [HttpDelete("{id:guid}")]
    [MustHavePermission(FSHAction.Delete, FSHResource.Category)]
    [OpenApiOperation("Delete a Category.", "")]
    public async Task<ActionResult> DeleteAsync(Guid id)
    {
        if (User.GetUserId() is not { } userId || string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        await _userService.DeleteAsync(id, userId);

        return Ok(await Result<Guid>.SuccessAsync(id, string.Format(_localizer["user.deleted"])));

    }

    [HttpPost("self-register")]
    [TenantIdHeader]
    [AllowAnonymous]
    [OpenApiOperation("Anonymous user creates a user.", "")]
    [ApiConventionMethod(typeof(FSHApiConventions), nameof(FSHApiConventions.Register))]
    public Task<string> SelfRegisterAsync(CreateUserRequest request)
    {
        // TODO: check if registering anonymous users is actually allowed (should probably be an appsetting)
        // and return UnAuthorized when it isn't
        // Also: add other protection to prevent automatic posting (captcha?)
        return _userService.CreateAsync(request, GetOriginFromRequest());
    }

    [HttpPost("{id}/toggle-status")]
    [MustHavePermission(FSHAction.Update, FSHResource.Users)]
    [ApiConventionMethod(typeof(FSHApiConventions), nameof(FSHApiConventions.Register))]
    [OpenApiOperation("Toggle a user's active status.", "")]
    public async Task<ActionResult> ToggleStatusAsync(string id, ToggleUserStatusRequest request, CancellationToken cancellationToken)
    {
        if (id != request.UserId)
        {
            return BadRequest();
        }

        await _userService.ToggleStatusAsync(request, cancellationToken);
        return Ok();
    }

    [HttpGet("confirm-email")]
    [AllowAnonymous]
    [OpenApiOperation("Confirm email address for a user.", "")]
    [ApiConventionMethod(typeof(FSHApiConventions), nameof(FSHApiConventions.Search))]
    public async Task<ActionResult> ConfirmEmailAsync([FromQuery] string tenant, [FromQuery] string userId, [FromQuery] string code, CancellationToken cancellationToken)
    {
        string result = await _userService.ConfirmEmailAsync(userId, code, tenant, cancellationToken);

        return Ok(await Result.SuccessAsync(result));
    }

    [HttpGet("confirm-phone-number")]
    [AllowAnonymous]
    [OpenApiOperation("Confirm phone number for a user.", "")]
    [ApiConventionMethod(typeof(FSHApiConventions), nameof(FSHApiConventions.Search))]
    public Task<string> ConfirmPhoneNumberAsync([FromQuery] string userId, [FromQuery] string code)
    {
        return _userService.ConfirmPhoneNumberAsync(userId, code);
    }

    [HttpPost("forgot-password")]
    [AllowAnonymous]
    [TenantIdHeader]
    [OpenApiOperation("Request a pasword reset email for a user.", "")]
    [ApiConventionMethod(typeof(FSHApiConventions), nameof(FSHApiConventions.Register))]
    public async Task<ActionResult> ForgotPasswordAsync(ForgotPasswordRequest request)
    {
        //string referer = Request.Headers["Referer"].ToString();
        //if (string.IsNullOrEmpty(referer))
        //    referer = GetOriginFromRequest();
        string result = await _userService.ForgotPasswordAsync(request, GetOriginFromRequest());

        return Ok(await Result.SuccessAsync(result));
    }

    [HttpPost("reset-password")]
    [AllowAnonymous]
    [TenantIdHeader]
    [OpenApiOperation("Reset a user's password.", "")]
    [ApiConventionMethod(typeof(FSHApiConventions), nameof(FSHApiConventions.Register))]
    public async Task<ActionResult> ResetPasswordAsync(ResetPasswordRequest request)
    {
        string result = await _userService.ResetPasswordAsync(request);
        return Ok(await Result.SuccessAsync(result));
    }

    private string GetOriginFromRequest()
    {
        return _configuration["ClientUrl"];
    }
    //=> $"{Request.Scheme}://{Request.Host.Value}{Request.PathBase.Value}";
}