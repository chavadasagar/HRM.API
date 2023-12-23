using HRM.API.Domain.Inventory;

namespace HRM.API.Application.Inventory;
public class DeletePurchasePaymentRequest : IRequest<string>
{
    public Guid Id { get; set; }

    public DeletePurchasePaymentRequest(Guid id) => Id = id;
}

public class DeletePurchasePaymentRequestHandler : IRequestHandler<DeletePurchasePaymentRequest, string>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<PurchasePayment> _repository;
    private readonly IStringLocalizer<DeletePurchasePaymentRequestHandler> _localizer;

    public DeletePurchasePaymentRequestHandler(IRepositoryWithEvents<PurchasePayment> repository, IStringLocalizer<DeletePurchasePaymentRequestHandler> localizer) =>
        (_repository,  _localizer) = (repository,  localizer);

    public async Task<string> Handle(DeletePurchasePaymentRequest request, CancellationToken cancellationToken)
    {
        var payment = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = payment ?? throw new NotFoundException(_localizer["purchase.notfoundpayment"]);

        await _repository.DeleteAsync(payment, cancellationToken);

        return payment.Id.ToString();
    }
}