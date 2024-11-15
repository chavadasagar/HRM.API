using MasterPOS.API.Domain.Common.Events;
using MasterPOS.API.Domain.Inventory;

namespace MasterPOS.API.Application.Catalog.Products;

public class DeleteProductRequest : IRequest<string>
{
    public Guid Id { get; set; }

    public DeleteProductRequest(Guid id) => Id = id;
}

public class DeleteProductRequestHandler : IRequestHandler<DeleteProductRequest, string>
{
    private readonly IRepository<Product> _repository;
    private readonly IReadRepository<PurchaseProduct> _purchaseRepo;
    private readonly IStringLocalizer<DeleteProductRequestHandler> _localizer;

    public DeleteProductRequestHandler(IRepository<Product> repository, IReadRepository<PurchaseProduct> purchaseRepo, IStringLocalizer<DeleteProductRequestHandler> localizer) =>
        (_repository, _purchaseRepo, _localizer) = (repository, purchaseRepo, localizer);

    public async Task<string> Handle(DeleteProductRequest request, CancellationToken cancellationToken)
    {
        if (await _purchaseRepo.AnyAsync(new PurchaseProductByProductSpec(request.Id), cancellationToken))
        {
            throw new ConflictException(_localizer["product.cannotbedeleted"]);
        }

        var product = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = product ?? throw new NotFoundException(_localizer["product.notfound"]);

        // Add Domain Events to be raised after the commit
        product.DomainEvents.Add(EntityDeletedEvent.WithEntity(product));

        await _repository.DeleteAsync(product, cancellationToken);

        return product.Name;
    }
}