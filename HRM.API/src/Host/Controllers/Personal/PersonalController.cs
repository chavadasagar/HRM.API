using System.Security.Claims;
using MasterPOS.API.Application.Auditing;
using MasterPOS.API.Application.Common.Wrapper;
using MasterPOS.API.Application.Identity.Users;
using MasterPOS.API.Application.Identity.Users.Password;
using Microsoft.Extensions.Localization;

namespace MasterPOS.API.Host.Controllers.Identity;

public class PersonalController : VersionNeutralApiController
{
    private readonly IUserService _userService;
    private readonly IStringLocalizer<PersonalController> _localizer;
    private readonly IConfiguration _configuration;
    public PersonalController(IUserService userService, IStringLocalizer<PersonalController> localizer, IConfiguration configuration) =>
        (_userService, _localizer, _configuration) = (userService, localizer, configuration);

    [HttpGet("profile")]
    [OpenApiOperation("Get profile details of currently logged in user.", "")]
    public async Task<ActionResult<UserDetailsDto>> GetProfileAsync(CancellationToken cancellationToken)
    {
        return User.GetUserId() is not { } userId || string.IsNullOrEmpty(userId)
            ? Unauthorized()
            : Ok(await _userService.GetAsync(userId, cancellationToken));
    }

    [HttpPut("profile")]
    [OpenApiOperation("Update profile details of currently logged in user.", "")]
    public async Task<ActionResult> UpdateProfileAsync(UpdateUserRequest request)
    {
        if (User.GetUserId() is not { } userId || string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        //string referer = Request.Headers["Referer"].ToString();
        //if (string.IsNullOrEmpty(referer))
        //    referer = GetOriginFromRequest();
        await _userService.UpdateProfileAsync(request, userId, GetOriginFromRequest());

        return Ok(await Result.SuccessAsync(_localizer["profile.update"]));
    }

    [HttpGet("roles")]
    [OpenApiOperation("Get a user's roles.", "")]
    public Task<List<UserRoleDto>> GetRolesAsync(CancellationToken cancellationToken)
    {
        if (User.GetUserId() is not { } userId || string.IsNullOrEmpty(userId))
        {
            return null;
        }

        return _userService.GetRolesAsync(userId, cancellationToken);
    }

    [HttpPut("change-password")]
    [OpenApiOperation("Change password of currently logged in user.", "")]
    [ApiConventionMethod(typeof(FSHApiConventions), nameof(FSHApiConventions.Register))]
    public async Task<ActionResult> ChangePasswordAsync(ChangePasswordRequest model)
    {
        if (User.GetUserId() is not { } userId || string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        await _userService.ChangePasswordAsync(model, userId);
        return Ok(await Result.SuccessAsync(_localizer["changepassword.success"]));
    }

    [HttpGet("permissions")]
    [OpenApiOperation("Get permissions of currently logged in user.", "")]
    public async Task<ActionResult<List<string>>> GetPermissionsAsync(CancellationToken cancellationToken)
    {
        return User.GetUserId() is not { } userId || string.IsNullOrEmpty(userId)
            ? Unauthorized()
            : Ok(await _userService.GetPermissionsAsync(userId, cancellationToken));
    }

    [HttpGet("logs")]
    [OpenApiOperation("Get audit logs of currently logged in user.", "")]
    public Task<List<AuditDto>> GetLogsAsync()
    {
        return Mediator.Send(new GetMyAuditLogsRequest());
    }

    [HttpPost("resend-confirm-email")]
    [AllowAnonymous]
    [OpenApiOperation("re-send Confirm email address for a user.", "")]
    public async Task<ActionResult> ReSendConfirmEmailAsync()
    {
        if (User.GetUserId() is not { } userId || string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }
        //string referer = Request.Headers["Referer"].ToString();
        //if (string.IsNullOrEmpty(referer))
        //    referer = GetOriginFromRequest();

        string result = await _userService.ReSendConfirmEmailAsync(userId, GetOriginFromRequest());

        return Ok(await Result.SuccessAsync(result));
    }

    private string GetOriginFromRequest()
    {
        return _configuration["ClientUrl"];
    }
}