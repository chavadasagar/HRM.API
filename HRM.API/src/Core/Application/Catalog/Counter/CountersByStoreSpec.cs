namespace HRM.API.Application.Catalog.Counters;

public class CountersByStoreSpec : Specification<Counter>
{
    public CountersByStoreSpec(Guid StoreId) =>
        Query.Where(p => p.StoreId == StoreId);
}
