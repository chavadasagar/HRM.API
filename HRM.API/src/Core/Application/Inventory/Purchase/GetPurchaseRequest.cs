using HRM.API.Domain.Inventory;

namespace HRM.API.Application.Inventory;

public class GetPurchaseRequest : IRequest<PurchaseDto>
{
    public Guid Id { get; set; }

    public GetPurchaseRequest(Guid id) => Id = id;
}

public class PurchaseByIdSpec : Specification<Purchase, PurchaseDto>, ISingleResultSpecification
{
    public PurchaseByIdSpec(Guid id) =>
        Query
            .Include(p => p.PurchaseProducts)
            .Include(p => p.PurchasePayments)
            .Where(p => p.Id == id);
}

public class GetPurchaseRequestHandler : IRequestHandler<GetPurchaseRequest, PurchaseDto>
{
    private readonly IRepository<Purchase> _repository;
    private readonly IStringLocalizer<GetPurchaseRequestHandler> _localizer;

    public GetPurchaseRequestHandler(IRepository<Purchase> repository, IStringLocalizer<GetPurchaseRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<PurchaseDto> Handle(GetPurchaseRequest request, CancellationToken cancellationToken)
    {
        return await _repository.GetBySpecAsync(
            (ISpecification<Purchase, PurchaseDto>)new PurchaseByIdSpec(request.Id), cancellationToken)
             ?? throw new NotFoundException(string.Format(_localizer["purchase.notfound"], request.Id));

    }
}