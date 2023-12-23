namespace HRM.API.Application.Catalog.Counters;

public class CounterByIdWithStoreSpec : Specification<Counter, CounterDetailsDto>, ISingleResultSpecification
{
    public CounterByIdWithStoreSpec(Guid id) =>
        Query
            .Where(p => p.Id == id)
            .Include(p => p.Store);
}