using MasterPOS.API.Domain.Inventory;

namespace MasterPOS.API.Application.Catalog.Suppliers;
public class PurchaseBySupplierSpec : Specification<Purchase>
{
    public PurchaseBySupplierSpec(Guid supplierId) =>
     Query.Where(p => p.SupplierId == supplierId);
}
