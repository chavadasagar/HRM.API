using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;

namespace Infrastructure.Test.Caching;

public class LocalCacheService : CacheService<MasterPOS.API.Infrastructure.Caching.LocalCacheService>
{
    protected override MasterPOS.API.Infrastructure.Caching.LocalCacheService CreateCacheService() =>
        new(
            new MemoryCache(new MemoryCacheOptions()),
            NullLogger<MasterPOS.API.Infrastructure.Caching.LocalCacheService>.Instance);
}