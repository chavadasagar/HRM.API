using MasterPOS.API.Domain.Inventory;

namespace MasterPOS.API.Application.Inventory;

public class GetPurchaseReturnRequest : IRequest<PurchaseReturnDto>
{
    public Guid Id { get; set; }

    public GetPurchaseReturnRequest(Guid id) => Id = id;
}

public class PurchaseReturnByIdSpec : Specification<PurchaseReturn, PurchaseReturnDto>, ISingleResultSpecification
{
    public PurchaseReturnByIdSpec(Guid id) =>
        Query
            .Include(p => p.PurchaseReturnProducts)
            .Include(p => p.PurchaseReturnPayments)
            .Where(p => p.Id == id);
}

public class GetPurchaseReturnRequestHandler : IRequestHandler<GetPurchaseReturnRequest, PurchaseReturnDto>
{
    private readonly IRepository<PurchaseReturn> _repository;
    private readonly IStringLocalizer<GetPurchaseReturnRequestHandler> _localizer;

    public GetPurchaseReturnRequestHandler(IRepository<PurchaseReturn> repository, IStringLocalizer<GetPurchaseReturnRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<PurchaseReturnDto> Handle(GetPurchaseReturnRequest request, CancellationToken cancellationToken)
    {
        return await _repository.GetBySpecAsync(
            (ISpecification<PurchaseReturn, PurchaseReturnDto>)new PurchaseReturnByIdSpec(request.Id), cancellationToken)
             ?? throw new NotFoundException(string.Format(_localizer["purchasereturn.notfound"], request.Id));

    }
}