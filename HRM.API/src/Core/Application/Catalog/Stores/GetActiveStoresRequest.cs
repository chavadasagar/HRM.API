namespace HRM.API.Application.Catalog.Stores;

public class GetActiveStoresRequest : IRequest<List<StoreDropdownDto>>
{
}

public class CountersRequestSpec : Specification<Store, StoreDropdownDto>
{
    public CountersRequestSpec()
    {
        Query
            .Where(c => c.IsActive)
            .OrderBy(c => c.Name);
    }
}

public class GetActiveStoresRequestHandler : IRequestHandler<GetActiveStoresRequest, List<StoreDropdownDto>>
{
    private readonly IReadRepository<Store> _repository;

    public GetActiveStoresRequestHandler(IReadRepository<Store> repository) => _repository = repository;
    public async Task<List<StoreDropdownDto>> Handle(GetActiveStoresRequest request, CancellationToken cancellationToken)
    {
        var spec = new CountersRequestSpec();
        return await _repository.ListAsync<StoreDropdownDto>(spec, cancellationToken);
    }
}