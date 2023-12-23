namespace HRM.API.Application.Catalog.Suppliers;

public class GetActiveSuppliersRequest : IRequest<List<SupplierDropdownDto>>
{
}

public class CountersRequestSpec : Specification<Supplier, SupplierDropdownDto>
{
    public CountersRequestSpec()
    {
        Query
            .Where(c => c.IsActive)
            .OrderBy(c => c.Name);
    }
}

public class GetActiveSuppliersRequestHandler : IRequestHandler<GetActiveSuppliersRequest, List<SupplierDropdownDto>>
{
    private readonly IReadRepository<Supplier> _repository;

    public GetActiveSuppliersRequestHandler(IReadRepository<Supplier> repository) => _repository = repository;
    public async Task<List<SupplierDropdownDto>> Handle(GetActiveSuppliersRequest request, CancellationToken cancellationToken)
    {
        var spec = new CountersRequestSpec();
        return await _repository.ListAsync<SupplierDropdownDto>(spec, cancellationToken);
    }
}