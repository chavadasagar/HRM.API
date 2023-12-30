namespace HRM.API.Application.Catalog.PaymentTypes;

public class DeletePaymentTypeRequest : IRequest<string>
{
    public Guid Id { get; set; }

    public DeletePaymentTypeRequest(Guid id) => Id = id;
}

public class DeletePaymentTypeRequestHandler : IRequestHandler<DeletePaymentTypeRequest, string>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<PaymentType> _repository;
    private readonly IStringLocalizer<DeletePaymentTypeRequestHandler> _localizer;
    private readonly IFileStorageService _file;

    public DeletePaymentTypeRequestHandler(IRepositoryWithEvents<PaymentType> repository, IStringLocalizer<DeletePaymentTypeRequestHandler> localizer, IFileStorageService file) =>
        (_repository, _localizer, _file) = (repository, localizer, file);

    public async Task<string> Handle(DeletePaymentTypeRequest request, CancellationToken cancellationToken)
    {
        var paymenttype = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = paymenttype ?? throw new NotFoundException(_localizer["paymenttype.notfound"]);

        // Remove old image if flag is set
        string? currentBrandImagePath = paymenttype.ImagePath;
        if (!string.IsNullOrEmpty(currentBrandImagePath))
        {
            string root = Directory.GetCurrentDirectory();
            _file.Remove(Path.Combine(root, currentBrandImagePath));
        }
        paymenttype = paymenttype.ClearImagePath();

        await _repository.DeleteAsync(paymenttype, cancellationToken);

        return paymenttype.Name;
    }
}