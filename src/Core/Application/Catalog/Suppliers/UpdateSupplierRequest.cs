using HRM.API.Domain.Inventory;

namespace HRM.API.Application.Catalog.Suppliers;

public class UpdateSupplierRequest : IRequest<Guid>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Mobile { get; set; } = default!;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? GSTNumber { get; set; }
    public string? BirthDate { get; set; }
    public string? AnniversaryDate { get; set; }
    public Guid? CountryId { get; set; }
    public Guid? StateId { get; set; }
    public string? City { get; set; }
    public string? Postcode { get; set; }
    public string? Address { get; set; }
    public bool IsActive { get; set; }
}

public class UpdateSupplierRequestValidator : CustomValidator<UpdateSupplierRequest>
{
    public UpdateSupplierRequestValidator(IRepository<Supplier> repository, IStringLocalizer<UpdateSupplierRequestValidator> localizer)
    {
        RuleFor(p => p.Mobile)
           .NotEmpty()
           .MaximumLength(256)
           .MustAsync(async (Supplier, mobile, ct) =>
                   await repository.GetBySpecAsync(new SupplierByMobileSpec(mobile), ct)
                       is not Supplier existingSupplier || existingSupplier.Id == Supplier.Id)
               .WithMessage((_, mobile) => string.Format(localizer["supplier.alreadyexists"], mobile));
    }
}

public class UpdateSupplierRequestHandler : IRequestHandler<UpdateSupplierRequest, Guid>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<Supplier> _repository;
    private readonly IReadRepository<Purchase> _purchaseRepo;
    private readonly IStringLocalizer<UpdateSupplierRequestHandler> _localizer;

    public UpdateSupplierRequestHandler(IRepositoryWithEvents<Supplier> repository, IReadRepository<Purchase> purchaseRepo, IStringLocalizer<UpdateSupplierRequestHandler> localizer) =>
        (_repository, _purchaseRepo, _localizer) = (repository, purchaseRepo, localizer);

    public async Task<Guid> Handle(UpdateSupplierRequest request, CancellationToken cancellationToken)
    {
        if (request.IsActive == false && (await _purchaseRepo.AnyAsync(new PurchaseBySupplierSpec(request.Id), cancellationToken)))
        {
            throw new ConflictException(_localizer["supplier.cannotbeinactive"]);
        }

        var supplier = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = supplier ?? throw new NotFoundException(string.Format(_localizer["supplier.notfound"], request.Id));

        supplier.Update(request.Name, request.Mobile, request.Email, request.Phone, request.GSTNumber, !string.IsNullOrEmpty(request.BirthDate) ? DateTime.Parse(request.BirthDate) : null, !string.IsNullOrEmpty(request.AnniversaryDate) ? DateTime.Parse(request.AnniversaryDate) : null, request.CountryId, request.StateId, request.City, request.Postcode, request.Address, request.IsActive);

        await _repository.UpdateAsync(supplier, cancellationToken);

        return request.Id;
    }
}