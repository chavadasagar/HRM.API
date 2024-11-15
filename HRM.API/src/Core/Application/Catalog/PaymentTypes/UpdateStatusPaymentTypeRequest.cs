namespace MasterPOS.API.Application.Catalog.PaymentTypes;
public class UpdateStatusPaymentTypeRequest : IRequest<Guid>
{
    public Guid Id { get; set; }
    public bool IsActive { get; set; }
}

public class UpdateStatusPaymentTypeRequestHandler : IRequestHandler<UpdateStatusPaymentTypeRequest, Guid>
{
    private readonly IRepositoryWithEvents<PaymentType> _repository;
    private readonly IStringLocalizer<UpdateStatusPaymentTypeRequestHandler> _localizer;

    public UpdateStatusPaymentTypeRequestHandler(IRepositoryWithEvents<PaymentType> repository, IStringLocalizer<UpdateStatusPaymentTypeRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<Guid> Handle(UpdateStatusPaymentTypeRequest request, CancellationToken cancellationToken)
    {
        var paymenttype = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = paymenttype ?? throw new NotFoundException(_localizer["paymenttype.notfound"]);
        paymenttype.IsActive = request.IsActive;
        await _repository.UpdateAsync(paymenttype, cancellationToken);
        return request.Id;
    }
}
