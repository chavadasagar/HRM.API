namespace HRM.API.Application.Catalog.Stores;

public class CreateStoreRequest : IRequest<Guid>
{
    public string Name { get; set; } = default!;
    public string? Mobile { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? GSTNumber { get; set; }
    public string? PANNumber { get; set; }
    public string? BankDetails { get; set; }
    public Guid? CountryId { get; set; }
    public Guid? StateId { get; set; }
    public string? City { get; set; }
    public string? Postcode { get; set; }
    public string? Address { get; set; }
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; }
    public FileUploadRequest? StoreLogoImage { get; set; }
}

public class CreateStoreRequestValidator : CustomValidator<CreateStoreRequest>
{
    public CreateStoreRequestValidator(IReadRepository<Store> repository, IStringLocalizer<CreateStoreRequestValidator> localizer)
    {
        RuleFor(p => p.Name)
                .NotEmpty()
                .MaximumLength(256)
                .MustAsync(async (name, ct) => await repository.GetBySpecAsync(new StoreByNameSpec(name), ct) is null)
                    .WithMessage((_, name) => string.Format(localizer["store.alreadyexists"], name));
    }

}

public class CreateStoreRequestHandler : IRequestHandler<CreateStoreRequest, Guid>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<Store> _repository;
    private readonly IStringLocalizer<CreateStoreRequestHandler> _localizer;
    private readonly IFileStorageService _file;
    private readonly IStoreService _storeService;

    public CreateStoreRequestHandler(IRepositoryWithEvents<Store> repository, IStringLocalizer<CreateStoreRequestHandler> localizer, IFileStorageService file, IStoreService storeService) =>
        (_repository, _localizer, _file, _storeService) = (repository, localizer, file, storeService);

    public async Task<Guid> Handle(CreateStoreRequest request, CancellationToken cancellationToken)
    {
        string storeImagePath = await _file.UploadAsync<Store>(request.StoreLogoImage, GlobalConstant.StoreImageUploadDirectory, FileType.Image, cancellationToken);

        string storeCode = await _storeService.GenerateStoreCode();
        var store = new Store(storeCode, request.Name, request.Mobile, request.Email, request.Phone, request.GSTNumber, request.PANNumber, request.BankDetails, request.CountryId, request.StateId, request.City, request.Postcode, request.Address, storeImagePath, request.IsDefault, request.IsActive);

        await _repository.AddAsync(store, cancellationToken);

        return store.Id;
    }
}