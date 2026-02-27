using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

namespace HRM.API.Infrastructure.FileStorage;

internal static class Startup
{
    internal static IApplicationBuilder UseFileStorage(this IApplicationBuilder app)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "Files");

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        return app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(path),
            RequestPath = new PathString("/Files")
        });
    }
}
