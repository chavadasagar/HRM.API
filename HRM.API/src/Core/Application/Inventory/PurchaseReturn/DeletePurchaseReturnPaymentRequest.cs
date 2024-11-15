using HRM.API.Domain.Inventory;

namespace HRM.API.Application.Inventory;
public class DeletePurchaseReturnPaymentRequest : IRequest<string>
{
    public Guid Id { get; set; }

    public DeletePurchaseReturnPaymentRequest(Guid id) => Id = id;
}

public class DeletePurchaseReturnPaymentRequestHandler : IRequestHandler<DeletePurchaseReturnPaymentRequest, string>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<PurchaseReturnPayment> _repository;
    private readonly IStringLocalizer<DeletePurchaseReturnPaymentRequestHandler> _localizer;

    public DeletePurchaseReturnPaymentRequestHandler(IRepositoryWithEvents<PurchaseReturnPayment> repository, IStringLocalizer<DeletePurchaseReturnPaymentRequestHandler> localizer) =>
        (_repository,  _localizer) = (repository,  localizer);

    public async Task<string> Handle(DeletePurchaseReturnPaymentRequest request, CancellationToken cancellationToken)
    {
        var payment = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = payment ?? throw new NotFoundException(_localizer["purchasereturn.notfoundpayment"]);

        await _repository.DeleteAsync(payment, cancellationToken);

        return payment.Id.ToString();
    }
}