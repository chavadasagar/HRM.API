namespace HRM.API.Application.Catalog.Counters;

public class CountersBySearchRequestWithStoresSpec : EntitiesByPaginationFilterSpec<Counter, CounterDto>
{
    public CountersBySearchRequestWithStoresSpec(SearchCountersRequest request)
        : base(request) =>
        Query
            .Include(p => p.Store)
            .OrderBy(c => c.Name, !request.HasOrderBy())
            .Where(p => p.StoreId.Equals(request.StoreId!.Value), request.StoreId.HasValue);
}