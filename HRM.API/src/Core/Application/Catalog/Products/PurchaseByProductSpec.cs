using MasterPOS.API.Domain.Inventory;

namespace MasterPOS.API.Application.Catalog.Products;
public class PurchaseProductByProductSpec : Specification<PurchaseProduct>
{
    public PurchaseProductByProductSpec(Guid productId) =>
      Query.Where(p => p.ProductId == productId);
}
