namespace MasterPOS.API.Application.Catalog.PaymentTypes;

public class UpdatePaymentTypeRequest : IRequest<Guid>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public FileUploadRequest? UploadRequest { get; set; }
}

public class UpdatePaymentTypeRequestValidator : CustomValidator<UpdatePaymentTypeRequest>
{
    public UpdatePaymentTypeRequestValidator(IRepository<PaymentType> repository, IStringLocalizer<UpdatePaymentTypeRequestValidator> localizer) =>
        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(75)
            .MustAsync(async (paymenttype, name, ct) =>
                    await repository.GetBySpecAsync(new PaymentTypeByNameSpec(name), ct)
                        is not PaymentType existingPaymentType || existingPaymentType.Id == paymenttype.Id)
                .WithMessage((_, name) => string.Format(localizer["paymenttype.alreadyexists"], name));
}

public class UpdatePaymentTypeRequestHandler : IRequestHandler<UpdatePaymentTypeRequest, Guid>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<PaymentType> _repository;
    private readonly IStringLocalizer<UpdatePaymentTypeRequestHandler> _localizer;
    private readonly IFileStorageService _file;

    public UpdatePaymentTypeRequestHandler(IRepositoryWithEvents<PaymentType> repository, IStringLocalizer<UpdatePaymentTypeRequestHandler> localizer, IFileStorageService file) =>
        (_repository, _localizer, _file) = (repository, localizer, file);

    public async Task<Guid> Handle(UpdatePaymentTypeRequest request, CancellationToken cancellationToken)
    {
        var paymenttype = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = paymenttype ?? throw new NotFoundException(string.Format(_localizer["paymenttype.notfound"], request.Id));

        // Remove old image if flag is set
        string? currentImagePath = paymenttype.ImagePath;
        if (request.UploadRequest != null && request.UploadRequest.Data != null)
        {
            if (!string.IsNullOrEmpty(currentImagePath))
            {
                string root = Directory.GetCurrentDirectory();
                _file.Remove(Path.Combine(root, currentImagePath));
            }
            paymenttype = paymenttype.ClearImagePath();
        }

        string? imagePath = request.UploadRequest is not null
            ? await _file.UploadAsync<PaymentType>(request.UploadRequest, GlobalConstant.PaymentTypeImageUploadDirectory, FileType.Image, cancellationToken)
            : null;

        paymenttype.Update(request.Name, request.Description, request.IsActive, imagePath ?? currentImagePath);

        await _repository.UpdateAsync(paymenttype, cancellationToken);

        return request.Id;
    }
}