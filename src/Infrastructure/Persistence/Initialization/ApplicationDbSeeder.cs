using HRM.API.Domain.Catalog;
using HRM.API.Domain.Configuration;
using HRM.API.Domain.Identity;
using HRM.API.Infrastructure.Identity;
using HRM.API.Infrastructure.Multitenancy;
using HRM.API.Infrastructure.Persistence.Context;
using HRM.API.Shared;
using HRM.API.Shared.Authorization;
using HRM.API.Shared.Multitenancy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace HRM.API.Infrastructure.Persistence.Initialization;

internal class ApplicationDbSeeder
{
    private readonly FSHTenantInfo _currentTenant;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly CustomSeederRunner _seederRunner;
    private readonly ILogger<ApplicationDbSeeder> _logger;

    public ApplicationDbSeeder(FSHTenantInfo currentTenant, RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, CustomSeederRunner seederRunner, ILogger<ApplicationDbSeeder> logger)
    {
        _currentTenant = currentTenant;
        _roleManager = roleManager;
        _userManager = userManager;
        _seederRunner = seederRunner;
        _logger = logger;
    }

    public async Task SeedDatabaseAsync(ApplicationDbContext dbContext, CancellationToken cancellationToken)
    {
        await SeedRolesAsync(dbContext);
        await SeedCountryAsync(dbContext);
        await SeedConfigurationAsync(dbContext);
        await SeedCompanyAsync(dbContext);
        await SeedAdminUserAsync();
        await _seederRunner.RunSeedersAsync(cancellationToken);
    }

    private async Task SeedRolesAsync(ApplicationDbContext dbContext)
    {
        foreach (string roleName in FSHRoles.DefaultRoles)
        {
            if (await _roleManager.Roles.SingleOrDefaultAsync(r => r.Name == roleName)
                is not ApplicationRole role)
            {
                // Create the role
                _logger.LogInformation("Seeding {role} Role for '{tenantId}' Tenant.", roleName, _currentTenant.Id);
                role = new ApplicationRole(roleName, $"{roleName} Role for {_currentTenant.Id} Tenant");
                await _roleManager.CreateAsync(role);
            }

            // Assign permissions
            if (roleName == FSHRoles.Basic)
            {
                await AssignPermissionsToRoleAsync(dbContext, FSHPermissions.Basic, role);
            }
            else if (roleName == FSHRoles.Admin)
            {
                await AssignPermissionsToRoleAsync(dbContext, FSHPermissions.Admin, role);

                if (_currentTenant.Id == MultitenancyConstants.Root.Id)
                {
                    await AssignPermissionsToRoleAsync(dbContext, FSHPermissions.Root, role);
                }
            }
        }
    }

    private async Task SeedCountryAsync(ApplicationDbContext dbContext)
    {
        string path = Path.Combine(Directory.GetCurrentDirectory(), "Configurations", "CountryStateCity.json");

        string json = File.ReadAllText(path);
        var allcountry = JsonConvert.DeserializeObject<List<Country>>(json);

        foreach (Country country in allcountry)
        {
            if (await dbContext.Countries.SingleOrDefaultAsync(r => r.Name == country.Name) == null)
            {
                _logger.LogInformation("Seeding '{countryname}' Country for '{tenantId}' Tenant.", country.Name, _currentTenant.Id);
                foreach (var state in country.States)
                {
                    _logger.LogInformation("Seeding '{statename}' State for '{tenantId}' Tenant.", state.Name, _currentTenant.Id);
                    foreach (var city in state.Cities)
                    {
                        _logger.LogInformation("Seeding '{statename}' City for '{tenantId}' Tenant.", city.Name, _currentTenant.Id);
                    }
                }
                await dbContext.Countries.AddAsync(new Country(country));
                await dbContext.SaveChangesAsync();
            }
        }
    }

    //private async Task SeedCountryAsync(ApplicationDbContext dbContext)
    //{
    //    foreach (string countryName in HRMCountry.Countries)
    //    {
    //        if (await dbContext.Countries.SingleOrDefaultAsync(r => r.Name == countryName) == null)
    //        {
    //            _logger.LogInformation("Seeding {countryName} Country for '{tenantId}' Tenant.", countryName, _currentTenant.Id);
    //            dbContext.Countries.Add(new Country(countryName));
    //            await dbContext.SaveChangesAsync();
    //        }
    //    }
    //}

    //private async Task SeedStateAsync(ApplicationDbContext dbContext)
    //{
    //    foreach (var state in HRMState.All)
    //    {
    //        if (await dbContext.States.SingleOrDefaultAsync(r => r.Name == state.stateName && r.Country.Name == state.countryName) == null)
    //        {
    //            var country = await dbContext.Countries.SingleOrDefaultAsync(r => r.Name == state.countryName);
    //            if (country != null)
    //            {
    //                dbContext.States.Add(new State(state.stateName, country.Id));
    //                await dbContext.SaveChangesAsync();
    //                _logger.LogInformation("Seeding {stateName} State for '{tenantId}' Tenant.", state.stateName, _currentTenant.Id);
    //            }
    //            else
    //            {
    //                _logger.LogInformation("Seeding {countryName} Country not found for '{tenantId}' Tenant.", state.countryName, _currentTenant.Id);
    //            }
    //        }
    //    }
    //}

    private async Task SeedConfigurationAsync(ApplicationDbContext dbContext)
    {
        foreach (var key in HRMConfigurations.All)
        {
            var config = await dbContext.GeneralConfigurations.Where(r => r.ConfigKey == key.ConfigKey).SingleOrDefaultAsync();
            if (config != null)
            {
                config.ConfigText = key.configText;
                config.SortOrder = key.sortOrder;
                dbContext.GeneralConfigurations.Update(config);
                await dbContext.SaveChangesAsync();
                _logger.LogInformation("Seeding {0} Configuration updated for '{tenantId}' Tenant.", key.ConfigKey, _currentTenant.Id);
            }
            else
            {
                dbContext.GeneralConfigurations.Add(new GeneralConfiguration(key.ConfigKey, key.ConfigText, key.ConfigValue, key.SortOrder));
                await dbContext.SaveChangesAsync();
                _logger.LogInformation("Seeding {0} Configuration added for '{tenantId}' Tenant.", key.ConfigKey, _currentTenant.Id);
            }
        }
    }

    private async Task AssignPermissionsToRoleAsync(ApplicationDbContext dbContext, IReadOnlyList<FSHPermission> permissions, ApplicationRole role)
    {
        var currentClaims = await _roleManager.GetClaimsAsync(role);
        foreach (var permission in permissions)
        {
            if (!currentClaims.Any(c => c.Type == FSHClaims.Permission && c.Value == permission.Name))
            {
                _logger.LogInformation("Seeding {role} Permission '{permission}' for '{tenantId}' Tenant.", role.Name, permission.Name, _currentTenant.Id);
                dbContext.RoleClaims.Add(new ApplicationRoleClaim
                {
                    RoleId = role.Id,
                    ClaimType = FSHClaims.Permission,
                    ClaimValue = permission.Name,
                    CreatedBy = "ApplicationDbSeeder"
                });
                await dbContext.SaveChangesAsync();
            }
        }
    }

    private async Task SeedCompanyAsync(ApplicationDbContext dbContext)
    {
        if (string.IsNullOrWhiteSpace(_currentTenant.Id) || string.IsNullOrWhiteSpace(_currentTenant.AdminEmail))
        {
            return;
        }

        if (await dbContext.Companies.FirstOrDefaultAsync(u => u.Email == _currentTenant.AdminEmail)
            is not Company company)
        {
            company = new Company(name: _currentTenant.Name.Trim().ToLowerInvariant(), directorName: null, email: _currentTenant.AdminEmail, mobile: null, phone: null, gSTNumber: null, vATNumber: null, pANNumber: null, website: null, uPIId: null, bankDetails: null, countryId: Guid.Empty, stateId: Guid.Empty, city: null, postcode: null, address: null, companyLogoPath: null, isActive: true);
            await dbContext.Companies.AddAsync(company);
            await dbContext.SaveChangesAsync();
            _logger.LogInformation("Seeding Comapny Profile for '{tenantId}' Tenant.", _currentTenant.Id);
        }
    }

    private async Task SeedAdminUserAsync()
    {
        if (string.IsNullOrWhiteSpace(_currentTenant.Id) || string.IsNullOrWhiteSpace(_currentTenant.AdminEmail))
        {
            return;
        }

        if (await _userManager.Users.FirstOrDefaultAsync(u => u.Email == _currentTenant.AdminEmail)
            is not ApplicationUser adminUser)
        {
            string adminUserName = $"{_currentTenant.Id.Trim()}.{FSHRoles.Admin}".ToLowerInvariant();
            adminUser = new ApplicationUser
            {
                FirstName = _currentTenant.Id.Trim().ToLowerInvariant(),
                LastName = FSHRoles.Admin,
                Email = _currentTenant.AdminEmail,
                UserName = Guid.NewGuid().ToString(), //request.UserName,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                NormalizedEmail = _currentTenant.AdminEmail?.ToUpperInvariant(),
                NormalizedUserName = adminUserName.ToUpperInvariant(),
                IsActive = true,
                IsPrimaryUser = true,
                CreatedOn = DateTime.Now
            };

            _logger.LogInformation("Seeding Default Admin User for '{tenantId}' Tenant.", _currentTenant.Id);
            var password = new PasswordHasher<ApplicationUser>();
            adminUser.PasswordHash = password.HashPassword(adminUser, MultitenancyConstants.DefaultPassword);
            await _userManager.CreateAsync(adminUser);
        }

        // Assign role to user
        if (!await _userManager.IsInRoleAsync(adminUser, FSHRoles.Admin))
        {
            _logger.LogInformation("Assigning Admin Role to Admin User for '{tenantId}' Tenant.", _currentTenant.Id);
            await _userManager.AddToRoleAsync(adminUser, FSHRoles.Admin);
        }
    }
}