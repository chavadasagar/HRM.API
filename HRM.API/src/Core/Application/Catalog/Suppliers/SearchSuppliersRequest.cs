 namespace MasterPOS.API.Application.Catalog.Suppliers;

public class SearchSuppliersRequest : PaginationFilter, IRequest<PaginationResponse<SupplierDto>>
{
    public Guid? StateId { get; set; }
    public Guid? CountryId { get; set; }
}

public class SuppliersBySearchRequestSpec : EntitiesByPaginationFilterSpec<Supplier, SupplierDto>
{
    public SuppliersBySearchRequestSpec(SearchSuppliersRequest request)
        : base(request) =>
        Query.Include(p => p.Country)
             .Include(p => p.State)
             .Where(p => p.CountryId.Equals(request.CountryId!.Value), request.CountryId.HasValue)
             .Where(p => p.StateId.Equals(request.StateId!.Value), request.StateId.HasValue)
             .OrderBy(c => c.Name, !request.HasOrderBy());
}

public class SearchSuppliersRequestHandler : IRequestHandler<SearchSuppliersRequest, PaginationResponse<SupplierDto>>
{
    private readonly IReadRepository<Supplier> _repository;

    public SearchSuppliersRequestHandler(IReadRepository<Supplier> repository) => _repository = repository;

    public async Task<PaginationResponse<SupplierDto>> Handle(SearchSuppliersRequest request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(request.Keyword))
            request.AdvancedSearch = new() { Fields = new List<string> { new string("country.name") }, Keyword = request.Keyword };

        var spec = new SuppliersBySearchRequestSpec(request);
        return await _repository.PaginatedListAsync(spec, request.PageNumber, request.PageSize, cancellationToken);
    }
}