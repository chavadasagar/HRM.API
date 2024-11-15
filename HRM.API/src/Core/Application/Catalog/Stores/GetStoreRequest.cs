namespace MasterPOS.API.Application.Catalog.Stores;

public class GetStoreRequest : IRequest<StoreDto>
{
    public Guid Id { get; set; }

    public GetStoreRequest(Guid id) => Id = id;
}

public class StoreByIdSpec : Specification<Store, StoreDto>, ISingleResultSpecification
{
    public StoreByIdSpec(Guid id) =>
        Query.Where(p => p.Id == id)
        .Include(p => p.Country)
        .Include(p => p.State);
}

public class GetStoreRequestHandler : IRequestHandler<GetStoreRequest, StoreDto>
{
    private readonly IRepository<Store> _repository;
    private readonly IStringLocalizer<GetStoreRequestHandler> _localizer;

    public GetStoreRequestHandler(IRepository<Store> repository, IStringLocalizer<GetStoreRequestHandler> localizer) => (_repository, _localizer) = (repository, localizer);

    public async Task<StoreDto> Handle(GetStoreRequest request, CancellationToken cancellationToken) =>
        await _repository.GetBySpecAsync(
            (ISpecification<Store, StoreDto>)new StoreByIdSpec(request.Id), cancellationToken)
        ?? throw new NotFoundException(string.Format(_localizer["store.notfound"], request.Id));
}