namespace HRM.API.Application.Catalog.PaymentTypes;

public class SearchPaymentTypesRequest : PaginationFilter, IRequest<PaginationResponse<PaymentTypeDto>>
{
}

public class PaymentTypesBySearchRequestSpec : EntitiesByPaginationFilterSpec<PaymentType, PaymentTypeDto>
{
    public PaymentTypesBySearchRequestSpec(SearchPaymentTypesRequest request)
        : base(request) =>
        Query.OrderBy(c => c.Name, !request.HasOrderBy());
}

public class SearchPaymentTypesRequestHandler : IRequestHandler<SearchPaymentTypesRequest, PaginationResponse<PaymentTypeDto>>
{
    private readonly IReadRepository<PaymentType> _repository;

    public SearchPaymentTypesRequestHandler(IReadRepository<PaymentType> repository) => _repository = repository;

    public async Task<PaginationResponse<PaymentTypeDto>> Handle(SearchPaymentTypesRequest request, CancellationToken cancellationToken)
    {
        var spec = new PaymentTypesBySearchRequestSpec(request);
        return await _repository.PaginatedListAsync(spec, request.PageNumber, request.PageSize, cancellationToken);
    }
}