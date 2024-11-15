using MasterPOS.API.Domain.Inventory;

namespace MasterPOS.API.Application.Inventory;

public class SearchPurchasesReturnRequest : PaginationFilter, IRequest<PaginationResponse<PurchaseReturnDto>>
{
}

public class SearchPurchasesReturnRequestSpec : EntitiesByPaginationFilterSpec<PurchaseReturn, PurchaseReturnDto>
{
    public SearchPurchasesReturnRequestSpec(SearchPurchasesReturnRequest request)
       : base(request) =>
       Query
           .Include(p => p.Store)
           .Include(p => p.Supplier)
           .OrderByDescending(c => c.PurchaseReturnDate, !request.HasOrderBy());
}

public class SearchPurchasesReturnRequestHandler : IRequestHandler<SearchPurchasesReturnRequest, PaginationResponse<PurchaseReturnDto>>
{
    private readonly IReadRepository<PurchaseReturn> _repository;

    public SearchPurchasesReturnRequestHandler(IReadRepository<PurchaseReturn> repository) => _repository = repository;

    public async Task<PaginationResponse<PurchaseReturnDto>> Handle(SearchPurchasesReturnRequest request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(request.Keyword))
            request.AdvancedSearch = new() { Fields = new List<string> { new string("supplier.name"), new string("store.name")}, Keyword = request.Keyword };

        var spec = new SearchPurchasesReturnRequestSpec(request);
        return await _repository.PaginatedListAsync(spec, request.PageNumber, request.PageSize, cancellationToken: cancellationToken);
    }
}