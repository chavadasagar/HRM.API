namespace HRM.API.Application.Catalog.PaymentTypes;
public class GetActivePaymentTypesRequest : IRequest<List<PaymentTypeDropdownDto>>
{

}

public class CountersRequestSpec : Specification<PaymentType, PaymentTypeDropdownDto>
{
    public CountersRequestSpec()
    {
        Query
            .Where(c => c.IsActive)
            .OrderBy(c => c.Name);
    }
}

public class GetActivePaymentTypesRequestHandler : IRequestHandler<GetActivePaymentTypesRequest, List<PaymentTypeDropdownDto>>
{
    private readonly IReadRepository<PaymentType> _repository;

    public GetActivePaymentTypesRequestHandler(IReadRepository<PaymentType> repository) => _repository = repository;
    public async Task<List<PaymentTypeDropdownDto>> Handle(GetActivePaymentTypesRequest request, CancellationToken cancellationToken)
    {
        var spec = new CountersRequestSpec();
        return await _repository.ListAsync<PaymentTypeDropdownDto>(spec, cancellationToken);
    }
}
