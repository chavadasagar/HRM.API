using MasterPOS.API.Domain.Inventory;

namespace MasterPOS.API.Application.Catalog.Products;
public class UpdateStatusProductRequest : IRequest<Guid>
{
    public Guid Id { get; set; }
    public bool IsActive { get; set; }
}

public class UpdateStatusProductRequestHandler : IRequestHandler<UpdateStatusProductRequest, Guid>
{
    private readonly IRepositoryWithEvents<Product> _repository;
    private readonly IReadRepository<PurchaseProduct> _purchaseProductRepo;
    private readonly IStringLocalizer<UpdateStatusProductRequestHandler> _localizer;

    public UpdateStatusProductRequestHandler(IRepositoryWithEvents<Product> repository, IReadRepository<PurchaseProduct> purchaseProductRepo, IStringLocalizer<UpdateStatusProductRequestHandler> localizer) =>
        (_repository, _purchaseProductRepo, _localizer) = (repository, purchaseProductRepo, localizer);

    public async Task<Guid> Handle(UpdateStatusProductRequest request, CancellationToken cancellationToken)
    {
        if (request.IsActive == false && (await _purchaseProductRepo.AnyAsync(new PurchaseProductByProductSpec(request.Id), cancellationToken)))
        {
            throw new ConflictException(_localizer["product.cannotbeinactive"]);
        }

        var product = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = product ?? throw new NotFoundException(_localizer["product.notfound"]);
        product.UpdateStatus(request.IsActive);
        await _repository.UpdateAsync(product, cancellationToken);
        return request.Id;
    }
}


