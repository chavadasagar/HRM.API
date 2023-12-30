using HRM.API.Application.Catalog;

namespace HRM.API.Domain.Catalog;

public class UpdateCategoryRequest : IRequest<Guid>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public FileUploadRequest? UploadRequest { get; set; }

}

public class UpdateCategoryRequestValidator : CustomValidator<UpdateCategoryRequest>
{
    public UpdateCategoryRequestValidator(IRepository<Category> repository, IStringLocalizer<UpdateCategoryRequestValidator> localizer) =>
            RuleFor(p => p.Name)
                .NotEmpty()
                .MaximumLength(75)
                .MustAsync(async (category, name, ct) =>
                    await repository.GetBySpecAsync(new CategoryByNameSpec(name), ct)
                        is not Category existingCategory || existingCategory.Id == category.Id)
                .WithMessage((_, name) => string.Format(localizer["category.alreadyexists"], name));

}


public class UpdateCategoryRequestHandler : IRequestHandler<UpdateCategoryRequest, Guid>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<Category> _repository;
    private readonly IReadRepository<Product> _productRepo;
    private readonly IStringLocalizer<UpdateCategoryRequestHandler> _localizer;
    private readonly IFileStorageService _file;

    public UpdateCategoryRequestHandler(IRepositoryWithEvents<Category> repository, IReadRepository<Product> productRepo, IStringLocalizer<UpdateCategoryRequestHandler> localizer, IFileStorageService file) =>
        (_repository, _productRepo, _localizer, _file) = (repository, productRepo, localizer, file);

    public async Task<Guid> Handle(UpdateCategoryRequest request, CancellationToken cancellationToken)
    {
        var category = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = category ?? throw new NotFoundException(string.Format(_localizer["category.notfound"], request.Id));

        if (request.IsActive == false && category.IsActive == true && (await _productRepo.AnyAsync(new ProductsByCategorySpec(request.Id), cancellationToken)))
        {
            throw new ConflictException(_localizer["category.cannotbeinactive"]);
        }

        // Remove old image if flag is set
        string? currentCategoryImagePath = category.ImagePath;
        if (request.UploadRequest != null && request.UploadRequest.Data != null)
        {
            if (!string.IsNullOrEmpty(currentCategoryImagePath))
            {
                string root = Directory.GetCurrentDirectory();
                _file.Remove(Path.Combine(root, currentCategoryImagePath));
            }
            category = category.ClearImagePath();
        }

        string? categoryImagePath = request.UploadRequest is not null
            ? await _file.UploadAsync<Category>(request.UploadRequest, GlobalConstant.CategoryImageUploadDirectory, FileType.Image, cancellationToken)
            : null;

        category.Update(request.Name, request.Description, request.IsActive, categoryImagePath ?? currentCategoryImagePath);

        await _repository.UpdateAsync(category, cancellationToken);

        return request.Id;
    }
}

