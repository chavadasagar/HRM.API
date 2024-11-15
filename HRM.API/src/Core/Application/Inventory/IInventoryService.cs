namespace MasterPOS.API.Application.Inventory;
public interface IInventoryService : ITransientService
{
    Task<decimal> GetProductStock(Guid productId, Guid storeId);
    Task<decimal> GetProductStock(Guid productId, Guid storeId, Guid typeId);
}
