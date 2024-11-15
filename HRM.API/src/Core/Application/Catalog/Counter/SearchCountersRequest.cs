namespace MasterPOS.API.Application.Catalog.Counters;

public class SearchCountersRequest : PaginationFilter, IRequest<PaginationResponse<CounterDto>>
{
    public Guid? StoreId { get; set; }
}

public class CountersBySearchRequestSpec : EntitiesByPaginationFilterSpec<Counter, CounterDto>
{
    public CountersBySearchRequestSpec(SearchCountersRequest request)
        : base(request)
    {
        Query
        .Include(x => x.Store)
        .Where(p => p.StoreId.Equals(request.StoreId!.Value), request.StoreId.HasValue)
        .OrderBy(c => c.Name, !request.HasOrderBy());
    }
}

public class SearchCountersRequestHandler : IRequestHandler<SearchCountersRequest, PaginationResponse<CounterDto>>
{
    private readonly IReadRepository<Counter> _repository;

    public SearchCountersRequestHandler(IReadRepository<Counter> repository) => _repository = repository;

    public async Task<PaginationResponse<CounterDto>> Handle(SearchCountersRequest request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(request.Keyword))
            request.AdvancedSearch = new() { Fields = new List<string> { new string("store.code"), new string("store.name") }, Keyword = request.Keyword };

        var spec = new CountersBySearchRequestSpec(request);
        return await _repository.PaginatedListAsync(spec, request.PageNumber, request.PageSize, cancellationToken: cancellationToken);
    }
}