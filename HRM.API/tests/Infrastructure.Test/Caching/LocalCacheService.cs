using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;

namespace Infrastructure.Test.Caching;

public class LocalCacheService : CacheService<HRM.API.Infrastructure.Caching.LocalCacheService>
{
    protected override HRM.API.Infrastructure.Caching.LocalCacheService CreateCacheService() =>
        new(
            new MemoryCache(new MemoryCacheOptions()),
            NullLogger<HRM.API.Infrastructure.Caching.LocalCacheService>.Instance);
}