using MasterPOS.API.Application.Inventory;
using MasterPOS.API.Infrastructure.Persistence.Context;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterPOS.API.Infrastructure.Inventory;
public class InventoryService : IInventoryService
{
    private readonly ApplicationDbContext _context;
    private readonly IStringLocalizer<InventoryService> _localizer;

    public InventoryService(
        ApplicationDbContext context,
        IStringLocalizer<InventoryService> localizer)
    {
        _context = context;
        _localizer = localizer;
    }

    public async Task<decimal> GetProductStock(Guid productId, Guid storeId)
    {
        decimal totalProductStock = 0;

        var products = _context.ProductQuantities.Where(x => x.ProductId == productId && x.StoreId == storeId);
        if (products.Any())
        {
            decimal totalPurchase = products.Where(x => x.Type == (short)ProductType.Purchase).Sum(x => x.Quantity);
            decimal totalPurchaseReturn = products.Where(x => x.Type == (short)ProductType.PurchaseReturn).Sum(x => x.Quantity);
            decimal totalSales = products.Where(x => x.Type == (short)ProductType.Sales).Sum(x => x.Quantity);

            totalProductStock = totalPurchase - totalPurchaseReturn + totalSales;
        }

        return totalProductStock;
    }

    public async Task<decimal> GetProductStock(Guid productId, Guid storeId, Guid typeId)
    {
        decimal totalProductStock = 0;
        if (typeId != Guid.Empty)
        {
            var products = _context.ProductQuantities.Where(x => x.ProductId == productId && x.StoreId == storeId );
            if (products.Any())
            {
                decimal totalPurchase = products.Where(x => x.Type == (short)ProductType.Purchase).Sum(x => x.Quantity);
                decimal totalPurchaseReturn = products.Where(x => x.Type == (short)ProductType.PurchaseReturn && x.TypeId != typeId).Sum(x => x.Quantity);
                decimal totalSales = products.Where(x => x.Type == (short)ProductType.Sales).Sum(x => x.Quantity);

                totalProductStock = totalPurchase - totalPurchaseReturn + totalSales;
            }
        }
        else
        {
            var products = _context.ProductQuantities.Where(x => x.ProductId == productId && x.StoreId == storeId);
            if (products.Any())
            {
                decimal totalPurchase = products.Where(x => x.Type == (short)ProductType.Purchase).Sum(x => x.Quantity);
                decimal totalPurchaseReturn = products.Where(x => x.Type == (short)ProductType.PurchaseReturn).Sum(x => x.Quantity);
                decimal totalSales = products.Where(x => x.Type == (short)ProductType.Sales).Sum(x => x.Quantity);

                totalProductStock = totalPurchase - totalPurchaseReturn + totalSales;
            }

        }

        return totalProductStock;
    }

}
