using MasterPOS.API.Application.Catalog.Products;
using MasterPOS.API.Domain.Common;
using MasterPOS.API.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace MasterPOS.API.Infrastructure.Common.Services;
public class ProductService : IProductService
{
    private readonly ApplicationDbContext _context = default!;

    public ProductService(ApplicationDbContext context) => _context = context;

    public async Task<long> GenerateProductCode()
    {
        long lastcode = 0;
        var product = await _context.Products
            .AsNoTracking()
            .OrderByDescending(a => a.PCode)
            .Take(1).SingleOrDefaultAsync();

        if (product != null)
        {
            lastcode = product.PCode;
        }

        lastcode = lastcode + 1;

        return lastcode;
    }
}
