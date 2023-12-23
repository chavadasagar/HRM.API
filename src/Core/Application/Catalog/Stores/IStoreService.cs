namespace HRM.API.Application.Catalog.Stores;
public interface IStoreService : IScopedService
{
   Task<string> GenerateStoreCode();
}
