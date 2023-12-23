namespace HRM.API.Application.Catalog.PaymentTypes;

public class CreatePaymentTypeRequest : IRequest<Guid>
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public FileUploadRequest? UploadRequest { get; set; }
}

public class CreatePaymentTypeRequestValidator : CustomValidator<CreatePaymentTypeRequest>
{
    public CreatePaymentTypeRequestValidator(IReadRepository<PaymentType> repository, IStringLocalizer<CreatePaymentTypeRequestValidator> localizer) =>
        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(75)
            .MustAsync(async (name, ct) => await repository.GetBySpecAsync(new PaymentTypeByNameSpec(name), ct) is null)
                .WithMessage((_, name) => string.Format(localizer["paymenttype.alreadyexists"], name));
}

public class CreatePaymentTypeRequestHandler : IRequestHandler<CreatePaymentTypeRequest, Guid>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<PaymentType> _repository;
    private readonly IStringLocalizer<CreatePaymentTypeRequestHandler> _localizer;
    private readonly IFileStorageService _file;
    public CreatePaymentTypeRequestHandler(IRepositoryWithEvents<PaymentType> repository, IStringLocalizer<CreatePaymentTypeRequestHandler> localizer,  IFileStorageService file) =>
        (_repository, _localizer, _file) = (repository, localizer, file);

    public async Task<Guid> Handle(CreatePaymentTypeRequest request, CancellationToken cancellationToken)
    {
        string imagePath = await _file.UploadAsync<PaymentType>(request.UploadRequest, GlobalConstant.PaymentTypeImageUploadDirectory, FileType.Image, cancellationToken);

        var paymenttype = new PaymentType(request.Name, request.Description, request.IsActive, imagePath);

        await _repository.AddAsync(paymenttype, cancellationToken);

        return paymenttype.Id;
    }
}