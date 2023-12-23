using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Finbuckle.MultiTenant;
using Mapster;
using HRM.API.Application.Common.Caching;
using HRM.API.Application.Common.Events;
using HRM.API.Application.Common.Exceptions;
using HRM.API.Application.Common.FileStorage;
using HRM.API.Application.Common.Interfaces;
using HRM.API.Application.Common.Mailing;
using HRM.API.Application.Common.Models;
using HRM.API.Application.Common.Specification;
using HRM.API.Application.Configuration;
using HRM.API.Application.Identity.Users;
using HRM.API.Domain.Identity;
using HRM.API.Infrastructure.Auth;
using HRM.API.Infrastructure.Mailing;
using HRM.API.Infrastructure.Persistence.Context;
using HRM.API.Shared.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace HRM.API.Infrastructure.Identity;

internal partial class UserService : IUserService
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly ApplicationDbContext _db;
    private readonly IStringLocalizer<UserService> _localizer;
    private readonly IJobService _jobService;
    private readonly IMailService _mailService;
    private readonly MailSettings _mailSettings;
    private readonly SecuritySettings _securitySettings;
    private readonly IEmailTemplateService _templateService;
    private readonly IFileStorageService _fileStorage;
    private readonly IEventPublisher _events;
    private readonly ICacheService _cache;
    private readonly ICacheKeyService _cacheKeys;
    private readonly ITenantInfo _currentTenant;
    private readonly IDataProtector _dataProtector;
    private readonly IConfiguration _configuration;

    public UserService(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        ApplicationDbContext db,
        IStringLocalizer<UserService> localizer,
        IJobService jobService,
        IMailService mailService,
        IOptions<MailSettings> mailSettings,
        IEmailTemplateService templateService,
        IFileStorageService fileStorage,
        IEventPublisher events,
        ICacheService cache,
        ICacheKeyService cacheKeys,
        ITenantInfo currentTenant,
        IOptions<SecuritySettings> securitySettings,
        IDataProtectionProvider dataProtectionProvider,
        IConfiguration configuration)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _roleManager = roleManager;
        _db = db;
        _localizer = localizer;
        _jobService = jobService;
        _mailService = mailService;
        _mailSettings = mailSettings.Value;
        _templateService = templateService;
        _fileStorage = fileStorage;
        _events = events;
        _cache = cache;
        _cacheKeys = cacheKeys;
        _currentTenant = currentTenant;
        _securitySettings = securitySettings.Value;
        _dataProtector = dataProtectionProvider.CreateProtector("DataProtectorTokenProvider");
        _configuration = configuration;
    }

    public async Task<PaginationResponse<UserDetailsDto>> SearchAsync(string userId, UserListFilter filter, CancellationToken cancellationToken)
    {
        var spec = new EntitiesByPaginationFilterSpec<ApplicationUser>(filter);
        var specCount = new EntitiesByBaseFilterSpec<ApplicationUser>(filter);

        var userList = await _userManager.Users.Where(x => x.Id != userId)
            .WithSpecification(spec)
            .ToListAsync(cancellationToken);

        var users = await _userManager.Users.Where(x => x.Id != userId)
            .WithSpecification(spec)
            .ProjectToType<UserDetailsDto>()
            .ToListAsync(cancellationToken);

        int count = await _userManager.Users.Where(x => x.Id != userId)
           .WithSpecification(specCount)
          .CountAsync(cancellationToken);

        foreach (var user in userList)
        {
            var role = await _userManager.GetRolesAsync(user);
            if (role.Count > 0)
            {
                users.Where(x => x.Id == new Guid(user.Id)).First().RoleName = role.First();
            }
        }

        return new PaginationResponse<UserDetailsDto>(users, count, filter.PageNumber, filter.PageSize);
    }

    public async Task<bool> ExistsWithNameAsync(string name)
    {
        EnsureValidTenant();
        return await _userManager.FindByNameAsync(name) is not null;
    }

    public async Task<bool> ExistsWithEmailAsync(string email, string? exceptId = null)
    {
        EnsureValidTenant();
        return await _userManager.FindByEmailAsync(email.Normalize()) is ApplicationUser user && user.Id != exceptId;
    }

    public async Task<bool> ExistsWithPhoneNumberAsync(string phoneNumber, string? exceptId = null)
    {
        EnsureValidTenant();
        return await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber) is ApplicationUser user && user.Id != exceptId;
    }

    private void EnsureValidTenant()
    {
        if (string.IsNullOrWhiteSpace(_currentTenant?.Id))
        {
            throw new UnauthorizedException(_localizer["tenant.invalid"]);
        }
    }

    public async Task<List<UserDetailsDto>> GetListAsync(CancellationToken cancellationToken) =>
        (await _userManager.Users
                .AsNoTracking()
                .ToListAsync(cancellationToken))
            .Adapt<List<UserDetailsDto>>();

    public Task<int> GetCountAsync(CancellationToken cancellationToken) =>
        _userManager.Users.AsNoTracking().CountAsync(cancellationToken);

    public async Task<UserDetailsDto> GetAsync(string userId, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users
            .AsNoTracking()
            .Where(u => u.Id == userId)
            .FirstOrDefaultAsync(cancellationToken);

        _ = user ?? throw new NotFoundException(_localizer["User Not Found."]);

        var userDetail = user.Adapt<UserDetailsDto>();
        if (user != null && userDetail != null)
        {
            var role = await _userManager.GetRolesAsync(user);
            if (role.Count > 0)
            {
                string roleName = role.First();
                var appRole = await _roleManager.FindByNameAsync(role.First());
                if (appRole != null)
                {
                    userDetail.RoleId = appRole.Id;
                    userDetail.RoleName = appRole.Name;
                }
            }
        }

        return userDetail;
    }

    public async Task ToggleStatusAsync(ToggleUserStatusRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users.Where(u => u.Id == request.UserId).FirstOrDefaultAsync(cancellationToken);

        _ = user ?? throw new NotFoundException(_localizer["User Not Found."]);

        bool isAdmin = await _userManager.IsInRoleAsync(user, FSHRoles.Admin);
        if (isAdmin)
        {
            throw new ConflictException(_localizer["Administrators Profile's Status cannot be toggled"]);
        }

        user.IsActive = request.ActivateUser;

        await _userManager.UpdateAsync(user);

        await _events.PublishAsync(new ApplicationUserUpdatedEvent(user.Id));
    }

    public async Task<CompanyModel> GetCompanyAsync()
    {
        var comapny = await (from c in _db.Companies
                             join country in _db.Countries on c.CountryId equals country.Id
                             join s in _db.States on c.StateId equals s.Id
                             select new CompanyModel
                             {
                                 Id = c.Id,
                                 Name = c.Name,
                                 DirectorName = c.DirectorName,
                                 Email = c.Email,
                                 Mobile = c.Mobile,
                                 Phone = c.Phone,
                                 GSTNumber = c.GSTNumber,
                                 VATNumber = c.VATNumber,
                                 PANNumber = c.PANNumber,
                                 Website = c.Website,
                                 UPIId = c.UPIId,
                                 BankDetails = c.BankDetails,
                                 CountryName = country.Name,
                                 StateName = s.Name,
                                 City = c.City,
                                 Postcode = c.Postcode,
                                 Address = c.Address,
                                 CompanyLogoPath = c.CompanyLogoPath
                             }).FirstOrDefaultAsync();

        return comapny;
    }
}