namespace HRM.API.Domain.Catalog;

public class CreateCategoryRequest : IRequest<Guid>
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public FileUploadRequest? UploadRequest { get; set; }
}

public class CreateCategoryRequestValidator : CustomValidator<CreateCategoryRequest>
{
    public CreateCategoryRequestValidator(IReadRepository<Category> repository, IStringLocalizer<CreateCategoryRequestValidator> localizer) =>
        RuleFor(p => p.Name)
        .NotEmpty()
        .MaximumLength(75)
        .MustAsync(async (name, ct) => await repository.GetBySpecAsync(new CategoryByNameSpec(name), ct) is null)
        .WithMessage((_, name) => string.Format(localizer["category.alreadyexists"], name));
}

public class CreateCategoryRequestHandler : IRequestHandler<CreateCategoryRequest, Guid>
{
    private readonly IRepositoryWithEvents<Category> _repository;
    private readonly IFileStorageService _file;
    public CreateCategoryRequestHandler(IRepositoryWithEvents<Category> repository, IFileStorageService file) =>
        (_repository, _file) = (repository, file);

    public async Task<Guid> Handle(CreateCategoryRequest request, CancellationToken cancellationToken)
    {
        string categoryImagePath = await _file.UploadAsync<Category>(request.UploadRequest, GlobalConstant.CategoryImageUploadDirectory, FileType.Image, cancellationToken);

        var category = new Category(request.Name, request.Description, request.IsActive, categoryImagePath);
        await _repository.AddAsync(category, cancellationToken);
        return category.Id;
    }
}
