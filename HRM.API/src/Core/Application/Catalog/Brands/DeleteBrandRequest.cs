using MasterPOS.API.Application.Catalog.Products;

namespace MasterPOS.API.Application.Catalog.Brands;

public class DeleteBrandRequest : IRequest<string>
{
    public Guid Id { get; set; }

    public DeleteBrandRequest(Guid id) => Id = id;
}

public class DeleteBrandRequestHandler : IRequestHandler<DeleteBrandRequest, string>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<Brand> _brandRepo;
    private readonly IReadRepository<Product> _productRepo;
    private readonly IStringLocalizer<DeleteBrandRequestHandler> _localizer;
    private readonly IFileStorageService _file;

    public DeleteBrandRequestHandler(IRepositoryWithEvents<Brand> brandRepo, IReadRepository<Product> productRepo, IStringLocalizer<DeleteBrandRequestHandler> localizer, IFileStorageService file) =>
        (_brandRepo, _productRepo, _localizer, _file) = (brandRepo, productRepo, localizer,file);

    public async Task<string> Handle(DeleteBrandRequest request, CancellationToken cancellationToken)
    {
        if (await _productRepo.AnyAsync(new ProductsByBrandSpec(request.Id), cancellationToken))
        {
            throw new ConflictException(_localizer["brand.cannotbedeleted"]);
        }

        var brand = await _brandRepo.GetByIdAsync(request.Id, cancellationToken);

        _ = brand ?? throw new NotFoundException(_localizer["brand.notfound"]);

        // Remove old image if flag is set
        string? currentBrandImagePath = brand.ImagePath;
        if (!string.IsNullOrEmpty(currentBrandImagePath))
        {
            string root = Directory.GetCurrentDirectory();
            _file.Remove(Path.Combine(root, currentBrandImagePath));
        }
        brand = brand.ClearImagePath();

        await _brandRepo.DeleteAsync(brand, cancellationToken);

        return brand.Name;
    }
}