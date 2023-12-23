using HRM.API.Domain.Common.Events;
using HRM.API.Domain.Inventory;

namespace HRM.API.Application.Catalog.Products;

public class UpdateProductRequest : IRequest<Guid>
{
    public string Name { get; set; } = default!;
    public Guid Id { get; set; }
    public Guid BrandId { get; set; }
    public Guid CategoryId { get; set; }
    public Guid UnitId { get; set; }
    public string? SKU { get; set; }
    public string? HSN { get; set; }
    public long? AlertQuantity { get; set; }
    public string? Barcode { get; set; }
    public string? Description { get; set; }
    public decimal BasePrice { get; set; }
    public decimal CGST { get; set; }
    public decimal SGST { get; set; }
    public decimal PurchasePrice { get; set; }
    public short TaxType { get; set; }
    public decimal ProfitMargin { get; set; }
    public decimal ProfitMarginAmount { get; set; }
    public decimal SalesPrice { get; set; }
    public bool IsActive { get; set; }
    public bool DeleteCurrentImage { get; set; } = false;
    public FileUploadRequest? Image { get; set; }
}

public class UpdateProductRequestHandler : IRequestHandler<UpdateProductRequest, Guid>
{
    private readonly IRepository<Product> _repository;
    private readonly IReadRepository<PurchaseProduct> _purchaseProductRepo;
    private readonly IStringLocalizer<UpdateProductRequestHandler> _localizer;
    private readonly IFileStorageService _file;

    public UpdateProductRequestHandler(IRepository<Product> repository, IReadRepository<PurchaseProduct> purchaseProductRepo, IStringLocalizer<UpdateProductRequestHandler> localizer, IFileStorageService file) =>
        (_repository, _purchaseProductRepo, _localizer, _file) = (repository, purchaseProductRepo, localizer, file);

    public async Task<Guid> Handle(UpdateProductRequest request, CancellationToken cancellationToken)
    {
        if (request.IsActive == false && (await _purchaseProductRepo.AnyAsync(new PurchaseProductByProductSpec(request.Id), cancellationToken)))
        {
            throw new ConflictException(_localizer["product.cannotbeinactive"]);
        }

        var product = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = product ?? throw new NotFoundException(string.Format(_localizer["product.notfound"], request.Id));

        // Remove old image if flag is set
        string? currentProductImagePath = product.ImagePath;
        if (request.Image != null && request.Image.Data != null)
        {
            if (!string.IsNullOrEmpty(currentProductImagePath))
            {
                string root = Directory.GetCurrentDirectory();
                _file.Remove(Path.Combine(root, currentProductImagePath));
            }
            product = product.ClearImagePath();
        }

        string? productImagePath = request.Image is not null
            ? await _file.UploadAsync<Product>(request.Image, GlobalConstant.UserImageUploadDirectory, FileType.Image, cancellationToken)
            : null;

        var updatedProduct = product.Update(name: request.Name, brandId: request.BrandId, categoryId: request.CategoryId, unitId: request.UnitId, sKU: request.SKU, hSN: request.HSN, alertQuantity: request.AlertQuantity, barcode: request.Barcode, description: request.Description, basePrice: request.BasePrice, cGST: request.CGST, sGST: request.SGST, purchasePrice: request.PurchasePrice, taxType: request.TaxType, profitMargin: request.ProfitMargin, profitMarginAmount: request.ProfitMarginAmount, salesPrice: request.SalesPrice, imagePath: productImagePath ?? currentProductImagePath, isActive: request.IsActive);

        // Add Domain Events to be raised after the commit
        product.DomainEvents.Add(EntityUpdatedEvent.WithEntity(product));

        await _repository.UpdateAsync(updatedProduct, cancellationToken);

        return request.Id;
    }
}