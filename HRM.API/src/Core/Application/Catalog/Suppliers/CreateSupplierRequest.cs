namespace MasterPOS.API.Application.Catalog.Suppliers;

public class CreateSupplierRequest : IRequest<Guid>
{
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

public class CreateSupplierRequestValidator : CustomValidator<CreateSupplierRequest>
{
    public CreateSupplierRequestValidator(IReadRepository<Supplier> repository, IStringLocalizer<CreateSupplierRequestValidator> localizer)
    {
        RuleFor(p => p.Mobile)
                .NotEmpty()
                .MaximumLength(256)
                .MustAsync(async (mobile, ct) => await repository.GetBySpecAsync(new SupplierByMobileSpec(mobile), ct) is null)
                    .WithMessage((_, mobile) => string.Format(localizer["supplier.alreadyexists"], mobile));
    }

}

public class CreateSupplierRequestHandler : IRequestHandler<CreateSupplierRequest, Guid>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<Supplier> _repository;
    private readonly IStringLocalizer<CreateSupplierRequestHandler> _localizer;

    public CreateSupplierRequestHandler(IRepositoryWithEvents<Supplier> repository, IStringLocalizer<CreateSupplierRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<Guid> Handle(CreateSupplierRequest request, CancellationToken cancellationToken)
    {
        var supplier = new Supplier(request.Name, request.Mobile, request.Email, request.Phone, request.GSTNumber, !string.IsNullOrEmpty(request.BirthDate) ? DateTime.Parse(request.BirthDate) : null, !string.IsNullOrEmpty(request.AnniversaryDate) ? DateTime.Parse(request.AnniversaryDate) : null, request.CountryId, request.StateId, request.City, request.Postcode, request.Address, request.IsActive);

        await _repository.AddAsync(supplier, cancellationToken);

        return supplier.Id;
    }
}