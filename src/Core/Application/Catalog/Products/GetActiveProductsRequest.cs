namespace HRM.API.Application.Catalog.Products;
public class GetActiveProductsRequest : IRequest<List<ProductDropdownDto>>
{
}

public class GetActiveProductsRequestSpec : Specification<Product, ProductDropdownDto>
{
    public GetActiveProductsRequestSpec()
    {
        Query
            .Where(c => c.IsActive)
            .OrderBy(c => c.Name);
    }
}

public class GetActiveProductsRequestHandler : IRequestHandler<GetActiveProductsRequest, List<ProductDropdownDto>>
{
    private readonly IReadRepository<Product> _repository;

    public GetActiveProductsRequestHandler(IReadRepository<Product> repository) => _repository = repository;
    public async Task<List<ProductDropdownDto>> Handle(GetActiveProductsRequest request, CancellationToken cancellationToken)
    {
        var spec = new GetActiveProductsRequestSpec();
        return await _repository.ListAsync<ProductDropdownDto>(spec, cancellationToken);
    }
}