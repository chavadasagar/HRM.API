using HRM.API.Domain.Identity;

namespace HRM.API.Application.Identity.Customers;

public class SearchCustomersRequest : PaginationFilter, IRequest<PaginationResponse<CustomerDto>>
{
    public Guid? StateId { get; set; }
    public Guid? CountryId { get; set; }
}

public class CustomersBySearchRequestSpec : EntitiesByPaginationFilterSpec<Customer, CustomerDto>
{
    public CustomersBySearchRequestSpec(SearchCustomersRequest request)
        : base(request) =>
        Query.Include(p => p.Country)
             .Include(p => p.State)
             .Where(p => p.CountryId.Equals(request.CountryId!.Value), request.CountryId.HasValue)
             .Where(p => p.StateId.Equals(request.StateId!.Value), request.StateId.HasValue)
             .OrderBy(c => c.Name, !request.HasOrderBy());
}

public class SearchCustomersRequestHandler : IRequestHandler<SearchCustomersRequest, PaginationResponse<CustomerDto>>
{
    private readonly IReadRepository<Customer> _repository;

    public SearchCustomersRequestHandler(IReadRepository<Customer> repository) => _repository = repository;

    public async Task<PaginationResponse<CustomerDto>> Handle(SearchCustomersRequest request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(request.Keyword))
            request.AdvancedSearch = new() { Fields = new List<string> { new string("country.name") }, Keyword = request.Keyword };

        var spec = new CustomersBySearchRequestSpec(request);
        return await _repository.PaginatedListAsync(spec, request.PageNumber, request.PageSize, cancellationToken);
    }
}