using MasterPOS.API.Domain.Identity;

namespace MasterPOS.API.Application.Identity.Customers;

public class GetCustomerRequest : IRequest<CustomerDto>
{
    public Guid Id { get; set; }

    public GetCustomerRequest(Guid id) => Id = id;
}

public class CustomerByIdSpec : Specification<Customer, CustomerDto>, ISingleResultSpecification
{
    public CustomerByIdSpec(Guid id) =>
        Query.Where(p => p.Id == id)
        .Include(p => p.Country)
        .Include(p => p.State);
}

public class GetCustomerRequestHandler : IRequestHandler<GetCustomerRequest, CustomerDto>
{
    private readonly IRepository<Customer> _repository;
    private readonly IStringLocalizer<GetCustomerRequestHandler> _localizer;

    public GetCustomerRequestHandler(IRepository<Customer> repository, IStringLocalizer<GetCustomerRequestHandler> localizer) => (_repository, _localizer) = (repository, localizer);

    public async Task<CustomerDto> Handle(GetCustomerRequest request, CancellationToken cancellationToken) =>
        await _repository.GetBySpecAsync(
            (ISpecification<Customer, CustomerDto>)new CustomerByIdSpec(request.Id), cancellationToken)
        ?? throw new NotFoundException(string.Format(_localizer["customer.notfound"], request.Id));
}