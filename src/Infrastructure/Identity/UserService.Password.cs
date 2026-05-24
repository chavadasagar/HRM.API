using HRM.API.Application.Common.Exceptions;
using HRM.API.Application.Common.Mailing;
using HRM.API.Application.Identity;
using HRM.API.Application.Identity.Users.Password;
using Microsoft.AspNetCore.WebUtilities;

namespace HRM.API.Infrastructure.Identity;

internal partial class UserService
{
    public async Task<string> ForgotPasswordAsync(ForgotPasswordRequest request, string origin)
    {
        EnsureValidTenant();

        var user = await _userManager.FindByEmailAsync(request.Email.Normalize());
        if (user is null) ///*|| !await _userManager.IsEmailConfirmedAsync(user)*/
        {
            // Don't reveal that the user does not exist or is not confirmed
            throw new InternalServerException(_localizer["forgotpassword.error"]);
        }

        if (user != null && user.IsActive == false)
        {
            throw new InternalServerException(_localizer["forgotpassword.errorforinactiveuser"]);
        }
        // For more information on how to enable account confirmation and password reset please
        // visit https://go.microsoft.com/fwlink/?LinkID=532713
        string code = await _userManager.GeneratePasswordResetTokenAsync(user);
        const string route = "auth/reset-password";
        var endpointUri = new Uri(string.Concat($"{origin}/", route));
        string passwordResetUrl = QueryHelpers.AddQueryString(endpointUri.ToString(), "Token", code);

        var companyDetail = await GetCompanyAsync();

        // send verification email
        ForgotPasswordEmailModel eMailModel = new ForgotPasswordEmailModel()
        {
            Email = user.Email,
            UserName = string.Format("{0} {1}", user.FirstName, user.LastName),
            ForgotLink = passwordResetUrl
        };

        if (companyDetail != null)
        {
            string apiUrl = _configuration["APIUrl"];

            eMailModel.CompanyLogoPath = string.Concat($"{apiUrl}/", companyDetail.CompanyLogoPath);
            eMailModel.CompanyName = companyDetail.Name;
            eMailModel.DirectorName = companyDetail.DirectorName;
            eMailModel.Website = companyDetail.Website;
            eMailModel.CompanyEmail = companyDetail.Email;
            eMailModel.Phone = companyDetail.Phone;
            eMailModel.Mobile = companyDetail.Mobile;
            eMailModel.CountryName = companyDetail.CountryName;
            eMailModel.StateName = companyDetail.StateName;
            eMailModel.City = companyDetail.City;
            eMailModel.Postcode = companyDetail.Postcode;
            eMailModel.Address = companyDetail.Address;
        }

        var mailRequest = new MailRequest(
            new List<string> { request.Email },
            _localizer["forgotpassword.emailsubject"],
            _templateService.GenerateEmailTemplate("forgot-password", eMailModel), displayName: eMailModel.CompanyName);
        _jobService.Enqueue(() => _mailService.SendAsync(mailRequest));

        return _localizer["forgotpassword.success"];
    }

    public async Task<string> ResetPasswordAsync(ResetPasswordRequest request)
    {
        string userId = string.Empty;

        // The password reset token comes from query.ResetToken
        var resetTokenArray = Convert.FromBase64String(request.Token);
        var unprotectedResetTokenArray = _dataProtector.Unprotect(resetTokenArray);

        using (var ms = new MemoryStream(unprotectedResetTokenArray))
        {
            using (var reader = new BinaryReader(ms))
            {
                // Read off the creation UTC timestamp
                reader.ReadInt64();

                // Then you can read the userId!
                userId = reader.ReadString();
            }
        }

        var user = await _userManager.FindByIdAsync(userId);

        // Don't reveal that the user does not exist
        _ = user ?? throw new InternalServerException(_localizer["resetpassword.error"]);

        var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);

        return result.Succeeded
            ? _localizer["Password Reset Successful!"]
            : throw new InternalServerException(_localizer["resetpassword.error"]);
    }

    public async Task ChangePasswordAsync(ChangePasswordRequest model, string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        _ = user ?? throw new NotFoundException(_localizer["User Not Found."]);

        var result = await _userManager.ChangePasswordAsync(user, model.Password, model.NewPassword);

        if (!result.Succeeded)
        {
            throw new InternalServerException(_localizer["Change password failed"], result.GetErrors(_localizer));
        }
    }
}