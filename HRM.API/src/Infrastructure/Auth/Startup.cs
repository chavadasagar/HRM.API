using HRM.API.Application.Common.Interfaces;
using HRM.API.Infrastructure.Auth.AzureAd;
using HRM.API.Infrastructure.Auth.Jwt;
using HRM.API.Infrastructure.Auth.Permissions;
using HRM.API.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HRM.API.Infrastructure.Auth;

internal static class Startup
{
    internal static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration config)
    {
        services
            .AddCurrentUser()
            .AddPermissions()

            // Must add identity before adding auth!
            .AddIdentity();
        services.Configure<SecuritySettings>(config.GetSection(nameof(SecuritySettings)));
        return config["SecuritySettings:Provider"].Equals("AzureAd", StringComparison.OrdinalIgnoreCase)
            ? services.AddAzureAdAuth(config)
            : services.AddJwtAuth(config);
    }

    internal static IApplicationBuilder UseCurrentUser(this IApplicationBuilder app) =>
        app.UseMiddleware<CurrentUserMiddleware>();

    private static IServiceCollection AddCurrentUser(this IServiceCollection services) =>
        services
            .AddScoped<CurrentUserMiddleware>()
            .AddScoped<ICurrentUser, CurrentUser>()
            .AddScoped(sp => (ICurrentUserInitializer)sp.GetRequiredService<ICurrentUser>());

    private static IServiceCollection AddPermissions(this IServiceCollection services) =>
        services
            .AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>()
            .AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
}