namespace MasterPOS.API.Application.Catalog;
public class GetActiveBrandsRequest : IRequest<List<BrandDropdownDto>>
{
}

public class BrandsRequestSpec : Specification<Brand, BrandDropdownDto>
{
    public BrandsRequestSpec()
    {
        Query
            .Where(c => c.IsActive)
            .OrderBy(c => c.Name);
    }
}

public class GetActiveBrandsRequestHandler : IRequestHandler<GetActiveBrandsRequest, List<BrandDropdownDto>>
{
    private readonly IReadRepository<Brand> _repository;

    public GetActiveBrandsRequestHandler(IReadRepository<Brand> repository) => _repository = repository;
    public async Task<List<BrandDropdownDto>> Handle(GetActiveBrandsRequest request, CancellationToken cancellationToken)
    {
        var spec = new BrandsRequestSpec();
        return await _repository.ListAsync<BrandDropdownDto>(spec, cancellationToken);
    }
}