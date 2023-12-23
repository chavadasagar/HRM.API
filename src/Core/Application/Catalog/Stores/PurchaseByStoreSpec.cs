using HRM.API.Domain.Inventory;

namespace HRM.API.Application.Catalog.Stores;
public class PurchaseByStoreSpec : Specification<Purchase>
{
    public PurchaseByStoreSpec(Guid StoreId) =>
       Query.Where(p => p.StoreId == StoreId);
}