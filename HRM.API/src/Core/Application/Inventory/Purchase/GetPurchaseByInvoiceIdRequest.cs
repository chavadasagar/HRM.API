using MasterPOS.API.Domain.Inventory;

namespace MasterPOS.API.Application.Inventory;
public class GetPurchaseByInvoiceIdRequest : IRequest<PurchaseDto>
{
    public string PurchaseInvoiceId { get; set; }

    public GetPurchaseByInvoiceIdRequest(string id) => PurchaseInvoiceId = id;
}

public class PurchaseByInvoiceIdSpec : Specification<Purchase, PurchaseDto>, ISingleResultSpecification
{
    public PurchaseByInvoiceIdSpec(string purchaseInvoiceId) =>
        Query
            .Include(p => p.PurchaseProducts)
            .Include(p => p.PurchasePayments)
            .Where(p => p.PurchaseInvoiceId == purchaseInvoiceId);
}

public class GetPurchaseByInvoiceIdRequestHandler : IRequestHandler<GetPurchaseByInvoiceIdRequest, PurchaseDto>
{
    private readonly IRepository<Purchase> _repository;
    private readonly IStringLocalizer<GetPurchaseByInvoiceIdRequestHandler> _localizer;

    public GetPurchaseByInvoiceIdRequestHandler(IRepository<Purchase> repository, IStringLocalizer<GetPurchaseByInvoiceIdRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<PurchaseDto> Handle(GetPurchaseByInvoiceIdRequest request, CancellationToken cancellationToken)
    {
        return await _repository.GetBySpecAsync(
            (ISpecification<Purchase, PurchaseDto>)new PurchaseByInvoiceIdSpec(request.PurchaseInvoiceId), cancellationToken)
             ?? throw new NotFoundException(string.Format(_localizer["purchase.notfound"], request.PurchaseInvoiceId));

    }
}