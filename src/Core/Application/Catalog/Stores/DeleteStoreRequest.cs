using HRM.API.Application.Catalog.Counters;
using HRM.API.Domain.Inventory;

namespace HRM.API.Application.Catalog.Stores;

public class DeleteStoreRequest : IRequest<string>
{
    public Guid Id { get; set; }

    public DeleteStoreRequest(Guid id) => Id = id;
}

public class DeleteStoreRequestHandler : IRequestHandler<DeleteStoreRequest, string>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<Store> _StoreRepo;
    private readonly IReadRepository<Counter> _counterRepo;
    private readonly IReadRepository<Purchase> _purchaseRepo;
    private readonly IStringLocalizer<DeleteStoreRequestHandler> _localizer;
    private readonly IFileStorageService _file;

    public DeleteStoreRequestHandler(IRepositoryWithEvents<Store> StoreRepo, IReadRepository<Counter> counterRepo, IReadRepository<Purchase> purchaseRepo, IStringLocalizer<DeleteStoreRequestHandler> localizer,IFileStorageService file) =>
        (_StoreRepo, _counterRepo, _purchaseRepo, _localizer, _file) = (StoreRepo, counterRepo, purchaseRepo, localizer,file);

    public async Task<string> Handle(DeleteStoreRequest request, CancellationToken cancellationToken)
    {
        if (await _counterRepo.AnyAsync(new CountersByStoreSpec(request.Id), cancellationToken))
        {
            throw new ConflictException(_localizer["store.cannotbedeleted"]);
        }

        if (await _purchaseRepo.AnyAsync(new PurchaseByStoreSpec(request.Id), cancellationToken))
        {
            throw new ConflictException(_localizer["store.cannotbedeleted"]);
        }

        var store = await _StoreRepo.GetByIdAsync(request.Id, cancellationToken);

        _ = store ?? throw new NotFoundException(_localizer["store.notfound"]);

        // Remove old image if flag is set
        string? currentStoreImagePath = store.StoreLogoPath;
        if (!string.IsNullOrEmpty(currentStoreImagePath))
        {
            string root = Directory.GetCurrentDirectory();
            _file.Remove(Path.Combine(root, currentStoreImagePath));
        }

        store = store.ClearImagePath();

        await _StoreRepo.DeleteAsync(store, cancellationToken);

        return store.Name;
    }
}