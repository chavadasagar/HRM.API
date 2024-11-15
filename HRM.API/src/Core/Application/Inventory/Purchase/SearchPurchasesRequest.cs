using MasterPOS.API.Domain.Inventory;

namespace MasterPOS.API.Application.Inventory;

public class SearchPurchasesRequest : PaginationFilter, IRequest<PaginationResponse<PurchaseDto>>
{
}

public class PurchasesBySearchRequestSpec : EntitiesByPaginationFilterSpec<Purchase, PurchaseDto>
{
    public PurchasesBySearchRequestSpec(SearchPurchasesRequest request)
       : base(request) =>
       Query
           .Include(p => p.Store)
           .Include(p => p.Supplier)
           .OrderByDescending(c => c.PurchaseDate, !request.HasOrderBy());
}

public class SearchPurchasesRequestHandler : IRequestHandler<SearchPurchasesRequest, PaginationResponse<PurchaseDto>>
{
    private readonly IReadRepository<Purchase> _repository;

    public SearchPurchasesRequestHandler(IReadRepository<Purchase> repository) => _repository = repository;

    public async Task<PaginationResponse<PurchaseDto>> Handle(SearchPurchasesRequest request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(request.Keyword))
            request.AdvancedSearch = new() { Fields = new List<string> { new string("supplier.name"), new string("store.name")}, Keyword = request.Keyword };

        var spec = new PurchasesBySearchRequestSpec(request);
        return await _repository.PaginatedListAsync(spec, request.PageNumber, request.PageSize, cancellationToken: cancellationToken);
    }
}