namespace MasterPOS.API.Application.Catalog;

public class GetActiveCategoriesRequest : IRequest<List<CategoryDropdownDto>>
{
}

public class CategoriesRequestSpec : Specification<Category, CategoryDropdownDto>
{
    public CategoriesRequestSpec()
    {
        Query
            .Where(c => c.IsActive)
            .OrderBy(c => c.Name);
    }
}

public class GetActiveCategoriesRequestHandler : IRequestHandler<GetActiveCategoriesRequest, List<CategoryDropdownDto>>
{
    private readonly IReadRepository<Category> _repository;

    public GetActiveCategoriesRequestHandler(IReadRepository<Category> repository) => _repository = repository;
    public async Task<List<CategoryDropdownDto>> Handle(GetActiveCategoriesRequest request, CancellationToken cancellationToken)
    {
        var spec = new CategoriesRequestSpec();
        return await _repository.ListAsync<CategoryDropdownDto>(spec, cancellationToken);
    }
}