using HRM.API.Application.Catalog.Counters;
using HRM.API.Domain.Inventory;

namespace HRM.API.Application.Catalog.Stores;

public class UpdateStoreRequest : IRequest<Guid>
{
    public Guid Id { get; set; }
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

public class UpdateStoreRequestValidator : CustomValidator<UpdateStoreRequest>
{
    public UpdateStoreRequestValidator(IRepository<Store> repository, IStringLocalizer<UpdateStoreRequestValidator> localizer)
    {
        RuleFor(p => p.Name)
           .NotEmpty()
           .MaximumLength(256)
           .MustAsync(async (store, name, ct) =>
                   await repository.GetBySpecAsync(new StoreByNameSpec(name), ct)
                       is not Store existingStore || existingStore.Id == store.Id)
               .WithMessage((_, name) => string.Format(localizer["store.alreadyexists"], name));
    }
}

public class UpdateStoreRequestHandler : IRequestHandler<UpdateStoreRequest, Guid>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<Store> _repository;
    private readonly IReadRepository<Counter> _counterRepo;
    private readonly IReadRepository<Purchase> _purchaseRepo;
    private readonly IStringLocalizer<UpdateStoreRequestHandler> _localizer;
    private readonly IFileStorageService _file;

    public UpdateStoreRequestHandler(IRepositoryWithEvents<Store> repository, IReadRepository<Counter> counterRepo, IReadRepository<Purchase> purchaseRepo, IStringLocalizer<UpdateStoreRequestHandler> localizer, IFileStorageService file) =>
        (_repository, _counterRepo, _purchaseRepo, _localizer) = (repository, counterRepo, purchaseRepo, localizer);

    public async Task<Guid> Handle(UpdateStoreRequest request, CancellationToken cancellationToken)
    {
        if (request.IsActive == false && (await _counterRepo.AnyAsync(new CountersByStoreSpec(request.Id), cancellationToken)))
        {
            throw new ConflictException(_localizer["store.cannotbeinactive"]);
        }

        if (request.IsActive == false && (await _purchaseRepo.AnyAsync(new PurchaseByStoreSpec(request.Id), cancellationToken)))
        {
            throw new ConflictException(_localizer["store.cannotbeinactive"]);
        }

        var store = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = store ?? throw new NotFoundException(string.Format(_localizer["store.notfound"], request.Id));

        // Remove old image if flag is set
        string? currentStoreLogoPath = store.StoreLogoPath;
        if (request.StoreLogoImage != null && request.StoreLogoImage.Data != null)
        {
            if (!string.IsNullOrEmpty(currentStoreLogoPath))
            {
                string root = Directory.GetCurrentDirectory();
                _file.Remove(Path.Combine(root, currentStoreLogoPath));
            }

            store = store.ClearImagePath();
        }

        string? storeImagePath = request.StoreLogoImage is not null
            ? await _file.UploadAsync<Store>(request.StoreLogoImage, GlobalConstant.StoreImageUploadDirectory, FileType.Image, cancellationToken)
            : null;

        store.Update(store.Code, request.Name, request.Mobile, request.Email, request.Phone, request.GSTNumber, request.PANNumber, request.BankDetails, request.CountryId, request.StateId, request.City, request.Postcode, request.Address, storeImagePath ?? currentStoreLogoPath, request.IsDefault, request.IsActive);

        await _repository.UpdateAsync(store, cancellationToken);

        return request.Id;
    }
}