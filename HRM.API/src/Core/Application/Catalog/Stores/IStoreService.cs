namespace MasterPOS.API.Application.Catalog.Stores;
public interface IStoreService : IScopedService
{
   Task<string> GenerateStoreCode();
}
