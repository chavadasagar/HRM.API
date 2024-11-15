using MasterPOS.API.Application.Catalog.Stores;
using MasterPOS.API.Domain.Common;
using MasterPOS.API.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace MasterPOS.API.Infrastructure.Common.Services;
public class StoreService : IStoreService
{
    private readonly ApplicationDbContext _context = default!;

    public StoreService(ApplicationDbContext context) => _context = context;

    public async Task<string> GenerateStoreCode()
    {
        int lastcode = 0;
        var store = await _context.Stores
            .AsNoTracking()
            .OrderByDescending(a => a.Code)
            .Take(1).SingleOrDefaultAsync();

        if (store != null)
        {
            string StoreCode = store.Code;
            StoreCode = StoreCode.Replace(GlobalConstant.PrefixStore, "");
            lastcode = Convert.ToInt32(StoreCode);
        }

        string newStoreCode = Convert.ToString(lastcode + 1).PadLeft(4, '0');
        return string.Format("{0}{1}", GlobalConstant.PrefixStore, newStoreCode);
    }
}
