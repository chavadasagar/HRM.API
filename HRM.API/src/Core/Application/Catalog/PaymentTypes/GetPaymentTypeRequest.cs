namespace MasterPOS.API.Application.Catalog.PaymentTypes;

public class GetPaymentTypeRequest : IRequest<PaymentTypeDto>
{
    public Guid Id { get; set; }

    public GetPaymentTypeRequest(Guid id) => Id = id;
}

public class PaymentTypeByIdSpec : Specification<PaymentType, PaymentTypeDto>, ISingleResultSpecification
{
    public PaymentTypeByIdSpec(Guid id) =>
        Query.Where(p => p.Id == id);
}

public class GetPaymentTypeRequestHandler : IRequestHandler<GetPaymentTypeRequest, PaymentTypeDto>
{
    private readonly IRepository<PaymentType> _repository;
    private readonly IStringLocalizer<GetPaymentTypeRequestHandler> _localizer;

    public GetPaymentTypeRequestHandler(IRepository<PaymentType> repository, IStringLocalizer<GetPaymentTypeRequestHandler> localizer) => (_repository, _localizer) = (repository, localizer);

    public async Task<PaymentTypeDto> Handle(GetPaymentTypeRequest request, CancellationToken cancellationToken) =>
        await _repository.GetBySpecAsync(
            (ISpecification<PaymentType, PaymentTypeDto>)new PaymentTypeByIdSpec(request.Id), cancellationToken)
        ?? throw new NotFoundException(string.Format(_localizer["paymenttype.notfound"], request.Id));
}