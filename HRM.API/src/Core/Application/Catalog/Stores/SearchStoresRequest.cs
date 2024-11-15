namespace MasterPOS.API.Application.Catalog.Stores;

public class SearchStoresRequest : PaginationFilter, IRequest<PaginationResponse<StoreDto>>
{
    public Guid? StateId { get; set; }
    public Guid? CountryId { get; set; }
}

public class StoresBySearchRequestSpec : EntitiesByPaginationFilterSpec<Store, StoreDto>
{
    public StoresBySearchRequestSpec(SearchStoresRequest request)
        : base(request) =>
        Query.Include(p => p.Country)
             .Include(p => p.State)
             .Where(p => p.CountryId.Equals(request.CountryId!.Value), request.CountryId.HasValue)
             .Where(p => p.StateId.Equals(request.StateId!.Value), request.StateId.HasValue)
             .OrderBy(c => c.Code, !request.HasOrderBy());
}

public class SearchStoresRequestHandler : IRequestHandler<SearchStoresRequest, PaginationResponse<StoreDto>>
{
    private readonly IReadRepository<Store> _repository;

    public SearchStoresRequestHandler(IReadRepository<Store> repository) => _repository = repository;

    public async Task<PaginationResponse<StoreDto>> Handle(SearchStoresRequest request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(request.Keyword))
            request.AdvancedSearch = new() { Fields = new List<string> { new string("country.name") }, Keyword = request.Keyword };

        var spec = new StoresBySearchRequestSpec(request);
        return await _repository.PaginatedListAsync(spec, request.PageNumber, request.PageSize, cancellationToken);
    }
}