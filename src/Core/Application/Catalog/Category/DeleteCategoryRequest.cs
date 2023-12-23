﻿using HRM.API.Application.Catalog;

namespace HRM.API.Domain.Catalog;

public class DeleteCategoryRequest : IRequest<string>
{
    public Guid Id { get; set; }

    public DeleteCategoryRequest(Guid Id) => this.Id = Id;
}

public class DeleteCategoryRequestHandler : IRequestHandler<DeleteCategoryRequest, string>
{
    private readonly IRepositoryWithEvents<Category> _categoryRepo;
    private readonly IReadRepository<Product> _productRepo;
    private readonly IStringLocalizer<DeleteCategoryRequestHandler> _localizer;
    private readonly IFileStorageService _file;

    public DeleteCategoryRequestHandler(IRepositoryWithEvents<Category> categoryRepo, IReadRepository<Product> productRepo, IStringLocalizer<DeleteCategoryRequestHandler> localizer, IFileStorageService file) =>
        (_categoryRepo, _productRepo, _localizer, _file) = (categoryRepo, productRepo, localizer, file);

    public async Task<string> Handle(DeleteCategoryRequest request, CancellationToken cancellationToken)
    {
        if (await _productRepo.AnyAsync(new ProductsByCategorySpec(request.Id), cancellationToken))
        {
            throw new ConflictException(_localizer["category.cannotbedeleted"]);
        }

        var category = await _categoryRepo.GetByIdAsync(request.Id);

        _ = category ?? throw new NotFoundException(_localizer["category.notfound"]);

        // Remove old image if flag is set
        string? currentCategoryImagePath = category.ImagePath;
        if (!string.IsNullOrEmpty(currentCategoryImagePath))
        {
            string root = Directory.GetCurrentDirectory();
            _file.Remove(Path.Combine(root, currentCategoryImagePath));
        }
        category = category.ClearImagePath();

        await _categoryRepo.DeleteAsync(category, cancellationToken);

        return category.Name;
    }
}
