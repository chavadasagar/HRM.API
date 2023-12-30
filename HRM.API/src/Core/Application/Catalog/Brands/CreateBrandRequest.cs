namespace HRM.API.Application.Catalog.Brands;

public class CreateBrandRequest : IRequest<Guid>
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public FileUploadRequest? UploadRequest { get; set; }
}

public class CreateBrandRequestValidator : CustomValidator<CreateBrandRequest>
{
    public CreateBrandRequestValidator(IReadRepository<Brand> repository, IStringLocalizer<CreateBrandRequestValidator> localizer) =>
        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(75)
            .MustAsync(async (name, ct) => await repository.GetBySpecAsync(new BrandByNameSpec(name), ct) is null)
                .WithMessage((_, name) => string.Format(localizer["brand.alreadyexists"], name));
}

public class CreateBrandRequestHandler : IRequestHandler<CreateBrandRequest, Guid>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<Brand> _repository;
    private readonly IStringLocalizer<CreateBrandRequestHandler> _localizer;
    private readonly IFileStorageService _file;

    public CreateBrandRequestHandler(IRepositoryWithEvents<Brand> repository, IStringLocalizer<CreateBrandRequestHandler> localizer, IFileStorageService file) =>
        (_repository, _localizer, _file) = (repository, localizer, file);

    public async Task<Guid> Handle(CreateBrandRequest request, CancellationToken cancellationToken)
    {
        string brandImagePath = await _file.UploadAsync<Brand>(request.UploadRequest, GlobalConstant.BrandImageUploadDirectory, FileType.Image, cancellationToken);

        var brand = new Brand(request.Name, request.Description, request.IsActive, brandImagePath);

        await _repository.AddAsync(brand, cancellationToken);

        return brand.Id;
    }
}