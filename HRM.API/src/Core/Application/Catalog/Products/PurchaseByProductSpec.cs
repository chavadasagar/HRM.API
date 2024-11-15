using HRM.API.Domain.Inventory;

namespace HRM.API.Application.Catalog.Products;
public class PurchaseProductByProductSpec : Specification<PurchaseProduct>
{
    public PurchaseProductByProductSpec(Guid productId) =>
      Query.Where(p => p.ProductId == productId);
}
