using HRM.API.Application.Catalog.Products;

namespace HRM.API.Application.Catalog.Brands;

public class UpdateBrandRequest : IRequest<Guid>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public FileUploadRequest? UploadRequest { get; set; }
}

public class UpdateBrandRequestValidator : CustomValidator<UpdateBrandRequest>
{
    public UpdateBrandRequestValidator(IRepository<Brand> repository, IStringLocalizer<UpdateBrandRequestValidator> localizer) =>
        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(75)
            .MustAsync(async (brand, name, ct) =>
                    await repository.GetBySpecAsync(new BrandByNameSpec(name), ct)
                        is not Brand existingBrand || existingBrand.Id == brand.Id)
                .WithMessage((_, name) => string.Format(localizer["brand.alreadyexists"], name));
}

public class UpdateBrandRequestHandler : IRequestHandler<UpdateBrandRequest, Guid>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<Brand> _repository;
    private readonly IReadRepository<Product> _productRepo;
    private readonly IStringLocalizer<UpdateBrandRequestHandler> _localizer;
    private readonly IFileStorageService _file;

    public UpdateBrandRequestHandler(IRepositoryWithEvents<Brand> repository, IReadRepository<Product> productRepo, IStringLocalizer<UpdateBrandRequestHandler> localizer, IFileStorageService file) =>
        (_repository, _productRepo, _localizer, _file) = (repository, productRepo, localizer, file);

    public async Task<Guid> Handle(UpdateBrandRequest request, CancellationToken cancellationToken)
    {
        var brand = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = brand ?? throw new NotFoundException(string.Format(_localizer["brand.notfound"], request.Id));

        if (request.IsActive == false && brand.IsActive == true && (await _productRepo.AnyAsync(new ProductsByBrandSpec(request.Id), cancellationToken)))
        {
            throw new ConflictException(_localizer["brand.cannotbeinactive"]);
        }

        // Remove old image if flag is set
        string? currentBrandImagePath = brand.ImagePath;
        if (request.UploadRequest != null && request.UploadRequest.Data != null)
        {
            if (!string.IsNullOrEmpty(currentBrandImagePath))
            {
                string root = Directory.GetCurrentDirectory();
                _file.Remove(Path.Combine(root, currentBrandImagePath));
            }
            brand = brand.ClearImagePath();
        }

        string? brandImagePath = request.UploadRequest is not null
            ? await _file.UploadAsync<Brand>(request.UploadRequest, GlobalConstant.BrandImageUploadDirectory, FileType.Image, cancellationToken)
            : null;


        brand.Update(request.Name, request.Description, request.IsActive, brandImagePath ?? currentBrandImagePath);

        await _repository.UpdateAsync(brand, cancellationToken);

        return request.Id;
    }
}