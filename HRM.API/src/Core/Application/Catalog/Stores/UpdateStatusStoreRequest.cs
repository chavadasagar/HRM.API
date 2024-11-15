using HRM.API.Application.Catalog.Counters;
using HRM.API.Domain.Inventory;

namespace HRM.API.Application.Catalog.Stores;
public class UpdateStatusStoreRequest : IRequest<Guid>
{
    public Guid Id { get; set; }
    public bool IsActive { get; set; }
}

public class UpdateStatusStoreRequestHandler : IRequestHandler<UpdateStatusStoreRequest, Guid>
{
    private readonly IRepositoryWithEvents<Store> _repository;
    private readonly IReadRepository<Counter> _counterRepo;
    private readonly IReadRepository<Purchase> _purchaseRepo;
    private readonly IStringLocalizer<UpdateStatusStoreRequestHandler> _localizer;

    public UpdateStatusStoreRequestHandler(IRepositoryWithEvents<Store> repository, IReadRepository<Counter> counterRepo, IReadRepository<Purchase> purchaseRepo, IStringLocalizer<UpdateStatusStoreRequestHandler> localizer) =>
        (_repository, _counterRepo, _purchaseRepo, _localizer) = (repository, counterRepo, purchaseRepo, localizer);

    public async Task<Guid> Handle(UpdateStatusStoreRequest request, CancellationToken cancellationToken)
    {
        if (request.IsActive == false && (await _counterRepo.AnyAsync(new CountersByStoreSpec(request.Id), cancellationToken)))
        {
            throw new ConflictException(_localizer["store.cannotbeinactive"]);
        }

        if (request.IsActive == false && (await _purchaseRepo.AnyAsync(new PurchaseByStoreSpec(request.Id), cancellationToken)))
        {
            throw new ConflictException(_localizer["store.cannotbeinactive"]);
        }

        var store = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = store ?? throw new NotFoundException(_localizer["store.notfound"]);
        store.UpdateStatus(request.IsActive);
        await _repository.UpdateAsync(store, cancellationToken);
        return request.Id;
    }
}
