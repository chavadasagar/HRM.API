namespace HRM.API.Application.Catalog.Counters;

public class CounterByNameSpec : Specification<Counter>, ISingleResultSpecification
{
    public CounterByNameSpec(string name, Guid storeId ) =>
        Query.Where(p => p.Name == name && p.StoreId == storeId);
}