using HRM.API.Domain.Inventory;

namespace HRM.API.Application.Catalog.Suppliers;
public class UpdateStatusSupplierRequest : IRequest<Guid>
{
    public Guid Id { get; set; }
    public bool IsActive { get; set; }
}

public class UpdateStatusSupplierRequestHandler : IRequestHandler<UpdateStatusSupplierRequest, Guid>
{
    private readonly IRepositoryWithEvents<Supplier> _repository;
    private readonly IReadRepository<Purchase> _purchaseRepo;
    private readonly IStringLocalizer<UpdateStatusSupplierRequestHandler> _localizer;

    public UpdateStatusSupplierRequestHandler(IRepositoryWithEvents<Supplier> repository, IReadRepository<Purchase> purchaseRepo, IStringLocalizer<UpdateStatusSupplierRequestHandler> localizer) =>
        (_repository, _purchaseRepo, _localizer) = (repository, purchaseRepo, localizer);

    public async Task<Guid> Handle(UpdateStatusSupplierRequest request, CancellationToken cancellationToken)
    {
        if (request.IsActive == false && (await _purchaseRepo.AnyAsync(new PurchaseBySupplierSpec(request.Id), cancellationToken)))
        {
            throw new ConflictException(_localizer["supplier.cannotbeinactive"]);
        }

        var supplier = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = supplier ?? throw new NotFoundException(_localizer["supplier.notfound"]);
        supplier.UpdateStatus(request.IsActive);
        await _repository.UpdateAsync(supplier, cancellationToken);
        return request.Id;
    }
}
