using MasterPOS.API.Domain.Inventory;

namespace MasterPOS.API.Application.Catalog.Suppliers;

public class DeleteSupplierRequest : IRequest<string>
{
    public Guid Id { get; set; }

    public DeleteSupplierRequest(Guid id) => Id = id;
}

public class DeleteSupplierRequestHandler : IRequestHandler<DeleteSupplierRequest, string>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<Supplier> _supplierRepo;
    private readonly IReadRepository<Purchase> _purchaseRepo;
    private readonly IStringLocalizer<DeleteSupplierRequestHandler> _localizer;

    public DeleteSupplierRequestHandler(IRepositoryWithEvents<Supplier> supplierRepo, IReadRepository<Purchase> purchaseRepo, IStringLocalizer<DeleteSupplierRequestHandler> localizer) =>
        (_supplierRepo, _purchaseRepo, _localizer) = (supplierRepo, purchaseRepo, localizer);

    public async Task<string> Handle(DeleteSupplierRequest request, CancellationToken cancellationToken)
    {
        if (await _purchaseRepo.AnyAsync(new PurchaseBySupplierSpec(request.Id), cancellationToken))
        {
            throw new ConflictException(_localizer["supplier.cannotbedeleted"]);
        }

        var supplier = await _supplierRepo.GetByIdAsync(request.Id, cancellationToken);

        _ = supplier ?? throw new NotFoundException(_localizer["supplier.notfound"]);

        await _supplierRepo.DeleteAsync(supplier, cancellationToken);

        return supplier.Name;
    }
}