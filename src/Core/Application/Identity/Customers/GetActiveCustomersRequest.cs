using HRM.API.Domain.Identity;

namespace HRM.API.Application.Identity.Customers;

public class GetActiveCustomersRequest : IRequest<List<CustomerDropdownDto>>
{
}

public class CountersRequestSpec : Specification<Customer, CustomerDropdownDto>
{
    public CountersRequestSpec()
    {
        Query
            .Where(c => c.IsActive)
            .OrderBy(c => c.Name);
    }
}

public class GetActiveStoresRequestHandler : IRequestHandler<GetActiveCustomersRequest, List<CustomerDropdownDto>>
{
    private readonly IReadRepository<Customer> _repository;

    public GetActiveStoresRequestHandler(IReadRepository<Customer> repository) => _repository = repository;
    public async Task<List<CustomerDropdownDto>> Handle(GetActiveCustomersRequest request, CancellationToken cancellationToken)
    {
        var spec = new CountersRequestSpec();
        return await _repository.ListAsync<CustomerDropdownDto>(spec, cancellationToken);
    }
}