using MasterPOS.API.Application.Common.Exceptions;
using MasterPOS.API.Application.Common.Mailing;
using MasterPOS.API.Application.Identity;
using MasterPOS.API.Application.Identity.Users;
using MasterPOS.API.Domain.Common;
using MasterPOS.API.Domain.Identity;
using MasterPOS.API.Shared.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using System.Security.Claims;

namespace MasterPOS.API.Infrastructure.Identity;

internal partial class UserService
{
    /// <summary>
    /// This is used when authenticating with AzureAd.
    /// The local user is retrieved using the objectidentifier claim present in the ClaimsPrincipal.
    /// If no such claim is found, an InternalServerException is thrown.
    /// If no user is found with that ObjectId, a new one is created and populated with the values from the ClaimsPrincipal.
    /// If a role claim is present in the principal, and the user is not yet in that roll, then the user is added to that role.
    /// </summary>
    public async Task<string> GetOrCreateFromPrincipalAsync(ClaimsPrincipal principal)
    {
        string? objectId = principal.GetObjectId();
        if (string.IsNullOrWhiteSpace(objectId))
        {
            throw new InternalServerException(_localizer["Invalid objectId"]);
        }

        var user = await _userManager.Users.Where(u => u.ObjectId == objectId).FirstOrDefaultAsync()
            ?? await CreateOrUpdateFromPrincipalAsync(principal);

        if (principal.FindFirstValue(ClaimTypes.Role) is string role &&
            await _roleManager.RoleExistsAsync(role) &&
            !await _userManager.IsInRoleAsync(user, role))
        {
            await _userManager.AddToRoleAsync(user, role);
        }

        return user.Id;
    }

    private async Task<ApplicationUser> CreateOrUpdateFromPrincipalAsync(ClaimsPrincipal principal)
    {
        string? email = principal.FindFirstValue(ClaimTypes.Upn);
        string? username = principal.GetDisplayName();
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(username))
        {
            throw new InternalServerException(string.Format(_localizer["Username or Email not valid."]));
        }

        var user = await _userManager.FindByNameAsync(username);
        if (user is not null && !string.IsNullOrWhiteSpace(user.ObjectId))
        {
            throw new InternalServerException(string.Format(_localizer["Username {0} is already taken."], username));
        }

        if (user is null)
        {
            user = await _userManager.FindByEmailAsync(email);
            if (user is not null && !string.IsNullOrWhiteSpace(user.ObjectId))
            {
                throw new InternalServerException(string.Format(_localizer["Email {0} is already taken."], email));
            }
        }

        IdentityResult? result;
        if (user is not null)
        {
            user.ObjectId = principal.GetObjectId();
            result = await _userManager.UpdateAsync(user);

            await _events.PublishAsync(new ApplicationUserUpdatedEvent(user.Id));
        }
        else
        {
            user = new ApplicationUser
            {
                ObjectId = principal.GetObjectId(),
                FirstName = principal.FindFirstValue(ClaimTypes.GivenName),
                LastName = principal.FindFirstValue(ClaimTypes.Surname),
                Email = email,
                NormalizedEmail = email.ToUpperInvariant(),
                UserName = username,
                NormalizedUserName = username.ToUpperInvariant(),
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                IsActive = true
            };
            result = await _userManager.CreateAsync(user);

            await _events.PublishAsync(new ApplicationUserCreatedEvent(user.Id));
        }

        if (!result.Succeeded)
        {
            throw new InternalServerException(_localizer["Validation Errors Occurred."], result.GetErrors(_localizer));
        }

        return user;
    }

    public async Task<string> CreateAsync(CreateUserRequest request, string origin)
    {
        var user = new ApplicationUser
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = Guid.NewGuid().ToString(), //request.UserName,
            PhoneNumber = request.PhoneNumber,
            PhoneNumberConfirmed = true,
            IsActive = request.IsActive,
            IsPrimaryUser = false,
            StoreId = request.StoreId,
            CreatedOn = DateTime.Now
        };
        if (request.Image != null)
        {
            user.ImageUrl = await _fileStorage.UploadAsync<ApplicationUser>(request.Image, GlobalConstant.UserImageUploadDirectory, FileType.Image);
        }

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            throw new InternalServerException(_localizer["user.validationerror"], result.GetErrors(_localizer));
        }

        var role = await _roleManager.FindByIdAsync(request.RoleId);
        if (role != null)
        {
            await _userManager.AddToRoleAsync(user, role.Name);
        }

        var messages = new List<string> { string.Format(_localizer["user.alreadyexists"], user.Email) };

        if (_securitySettings.RequireConfirmedAccount && !string.IsNullOrEmpty(user.Email))
        {
            var companyDetail = await GetCompanyAsync();
            // send verification email
            string emailVerificationUri = await GetEmailVerificationUriAsync(user, origin);
            RegisterUserEmailModel eMailModel = new RegisterUserEmailModel()
            {
                Email = user.Email,
                UserName = user.UserName,
                Url = emailVerificationUri,
                FullName = user.FirstName + ' ' + user.LastName,
                RoleName = role?.Name,
                Password = request.Password
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
                new List<string> { user.Email },
                _localizer["user.confirmregistration"],
                _templateService.GenerateEmailTemplate("email-confirmation-welcome", eMailModel), displayName: eMailModel.CompanyName);
            _jobService.Enqueue(() => _mailService.SendAsync(mailRequest));
            messages.Add(string.Format(_localizer["user.emailconfirm"], user.Email));
        }

        await _events.PublishAsync(new ApplicationUserCreatedEvent(user.Id));

        return string.Join(Environment.NewLine, messages);
    }

    public async Task<string> UpdateAsync(UpdateUserRequest request, string userId, string origin)
    {
        var user = await _userManager.FindByIdAsync(userId);

        _ = user ?? throw new NotFoundException(_localizer["user.notfound"]);

        string currentImage = user.ImageUrl ?? string.Empty;
        if (request.Image != null || request.DeleteCurrentImage)
        {
            user.ImageUrl = await _fileStorage.UploadAsync<ApplicationUser>(request.Image, GlobalConstant.UserImageUploadDirectory, FileType.Image);
            if (!string.IsNullOrEmpty(currentImage))
            {
                string root = Directory.GetCurrentDirectory();
                _fileStorage.Remove(Path.Combine(root, currentImage));
            }
        }

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.PhoneNumber = request.PhoneNumber;
        user.IsActive = request.IsActive;

        string email = user.Email; // await _userManager.GetEmailAsync(user);
        if (request.Email != email)
        {
            user.Email = request.Email;
            await _userManager.SetEmailAsync(user, request.Email);

            var companyDetail = await GetCompanyAsync();

            // send new verification email
            string emailVerificationUri = await GetEmailVerificationUriAsync(user, origin);
            RegisterUserEmailModel eMailModel = new RegisterUserEmailModel()
            {
                Email = user.Email,
                UserName = user.UserName,
                Url = emailVerificationUri,
                FullName = user.FirstName + ' ' + user.LastName
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

            //old email
            var oldmailRequest = new MailRequest(
               new List<string> { email },
               _localizer["user.emailchangenotification"],
               _templateService.GenerateEmailTemplate("email-confirmation-oldemail", eMailModel), displayName: eMailModel.CompanyName);
            _jobService.Enqueue(() => _mailService.SendAsync(oldmailRequest));

            var mailRequest = new MailRequest(
                new List<string> { request.Email },
                _localizer["user.emailchangenotification"],
                _templateService.GenerateEmailTemplate("email-confirmation-newemail", eMailModel), displayName: eMailModel.CompanyName);
            _jobService.Enqueue(() => _mailService.SendAsync(mailRequest));
        }

        user.StoreId = request.StoreId;
        var result = await _userManager.UpdateAsync(user);

        if (request.Password != null && !string.IsNullOrEmpty(request.Password))
        {
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            await _userManager.ResetPasswordAsync(user, code, request.Password);

            var companyDetail = await GetCompanyAsync();
            RegisterUserEmailModel eMailModel = new RegisterUserEmailModel()
            {
                Email = user.Email,
                UserName = user.UserName,
                FullName = user.FirstName + ' ' + user.LastName,
                Password = request.Password
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

            //password change notification email
            var oldmailRequest = new MailRequest(
               new List<string> { user.Email },
               _localizer["user.confirmemailupdate"],
               _templateService.GenerateEmailTemplate("password-change-notification", eMailModel), displayName: eMailModel.CompanyName);
            _jobService.Enqueue(() => _mailService.SendAsync(oldmailRequest));

        }

        if (request.RoleId != null)
        {
            var role = await _userManager.GetRolesAsync(user);
            if (role.Count > 0)
            {
                await _userManager.RemoveFromRoleAsync(user, role.First());
            }

            var roleAdd = await _roleManager.FindByIdAsync(request.RoleId);
            if (roleAdd != null)
            {
                await _userManager.AddToRoleAsync(user, roleAdd.Name);
            }
        }

        await _signInManager.RefreshSignInAsync(user);

        await _events.PublishAsync(new ApplicationUserUpdatedEvent(user.Id));

        if (!result.Succeeded)
        {
            throw new InternalServerException(_localizer["user.updatefailed"], result.GetErrors(_localizer));
        }

        return string.Format(_localizer["user.updated"], user.Email);
    }

    public async Task<string> UpdateProfileAsync(UpdateUserRequest request, string userId, string origin)
    {
        var user = await _userManager.FindByIdAsync(userId);

        _ = user ?? throw new NotFoundException(_localizer["user.notfound"]);

        string currentImage = user.ImageUrl ?? string.Empty;
        if (request.Image != null || request.DeleteCurrentImage)
        {
            user.ImageUrl = await _fileStorage.UploadAsync<ApplicationUser>(request.Image, GlobalConstant.UserImageUploadDirectory, FileType.Image);
            if (!string.IsNullOrEmpty(currentImage))
            {
                string root = Directory.GetCurrentDirectory();
                _fileStorage.Remove(Path.Combine(root, currentImage));
            }
        }

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.PhoneNumber = request.PhoneNumber;

        string email = await _userManager.GetEmailAsync(user);
        if (request.Email != email)
        {
            await _userManager.SetEmailAsync(user, request.Email);

            var companyDetail = await GetCompanyAsync();

            // send verification email
            string emailVerificationUri = await GetEmailVerificationUriAsync(user, origin);
            RegisterUserEmailModel eMailModel = new RegisterUserEmailModel()
            {
                Email = user.Email,
                UserName = user.UserName,
                Url = emailVerificationUri,
                FullName = user.FirstName + ' ' + user.LastName
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
                new List<string> { user.Email },
                _localizer["user.emailchangenotification"],
                _templateService.GenerateEmailTemplate("email-confirmation-verification", eMailModel), displayName: eMailModel.CompanyName);
            _jobService.Enqueue(() => _mailService.SendAsync(mailRequest));

        }

        var result = await _userManager.UpdateAsync(user);

        await _signInManager.RefreshSignInAsync(user);

        await _events.PublishAsync(new ApplicationUserUpdatedEvent(user.Id));

        if (!result.Succeeded)
        {
            throw new InternalServerException(_localizer["user.updatefailed"], result.GetErrors(_localizer));
        }

        return string.Format(_localizer["user.updated"], user.Email);
    }

    public async Task DeleteAsync(Guid Id, string userId)
    {
        var user = await _userManager.FindByIdAsync(Id.ToString());

        _ = user ?? throw new NotFoundException(_localizer["user.notfound"]);

        string currentImage = user.ImageUrl ?? string.Empty;
        if (!string.IsNullOrEmpty(currentImage))
        {
            string root = Directory.GetCurrentDirectory();
            _fileStorage.Remove(Path.Combine(root, currentImage));
        }

        user.DeletedBy = new Guid(userId);
        user.DeletedOn = DateTime.Now;
        var result = await _userManager.UpdateAsync(user);

        await _signInManager.RefreshSignInAsync(user);

        await _events.PublishAsync(new ApplicationUserUpdatedEvent(user.Id));

        if (!result.Succeeded)
        {
            throw new InternalServerException(_localizer["deleteuserfailed"], result.GetErrors(_localizer));
        }
    }
    public async Task UpdateStatusAsync(UpdateUserStatusRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.Id);

        _ = user ?? throw new NotFoundException(_localizer["user.notfound"]);

        user.IsActive = request.IsActive;
        var result = await _userManager.UpdateAsync(user);

        await _signInManager.RefreshSignInAsync(user);

        await _events.PublishAsync(new ApplicationUserUpdatedEvent(user.Id));

        if (!result.Succeeded)
        {
            throw new InternalServerException(_localizer["user.updatestatusfailed"], result.GetErrors(_localizer));
        }
    }

    public async Task<string> ReSendConfirmEmailAsync(string userId, string origin)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            var companyDetail = await GetCompanyAsync();

            // send verification email
            string emailVerificationUri = await GetEmailVerificationUriAsync(user, origin);
            RegisterUserEmailModel eMailModel = new RegisterUserEmailModel()
            {
                Email = user.Email,
                UserName = user.UserName,
                Url = emailVerificationUri,
                FullName = user.FirstName + ' ' + user.LastName
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
                new List<string> { user.Email },
                _localizer["user.confirmemailupdate"],
                _templateService.GenerateEmailTemplate("email-confirmation-newemail", eMailModel), displayName: eMailModel.CompanyName);
            _jobService.Enqueue(() => _mailService.SendAsync(mailRequest));

            return string.Format(_localizer["user.emailconfirm"], user.Email);
        }

        return _localizer["user.notfound"];
    }
}
