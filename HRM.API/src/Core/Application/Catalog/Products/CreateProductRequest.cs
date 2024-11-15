using MasterPOS.API.Domain.Common.Events;

namespace MasterPOS.API.Application.Catalog.Products;

public class CreateProductRequest : IRequest<Guid>
{
    public string Name { get; set; } = default!;
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
    public FileUploadRequest? Image { get; set; }
}

public class CreateProductRequestHandler : IRequestHandler<CreateProductRequest, Guid>
{
    private readonly IRepository<Product> _repository;
    private readonly IFileStorageService _file;
    private readonly IProductService _productService;

    public CreateProductRequestHandler(IRepository<Product> repository, IFileStorageService file, IProductService productService) =>
        (_repository, _file, _productService) = (repository, file, productService);

    public async Task<Guid> Handle(CreateProductRequest request, CancellationToken cancellationToken)
    {
        string productImagePath = await _file.UploadAsync<Product>(request.Image, GlobalConstant.ProductImageUploadDirectory, FileType.Image, cancellationToken);

        long pCode = await _productService.GenerateProductCode();

        string productCode = string.Empty;
        if (pCode >= 999)
            productCode = Convert.ToString(pCode);
        else
            productCode = Convert.ToString(pCode).PadLeft(4, '0');

        productCode = string.Format("{0}{1}", GlobalConstant.PrefixProduct, productCode);

        var product = new Product(name: request.Name, pCode: pCode, code: productCode, brandId: request.BrandId, categoryId: request.CategoryId, unitId: request.UnitId, sKU: request.SKU, hSN: request.HSN, alertQuantity: request.AlertQuantity, barcode: request.Barcode, description: request.Description, basePrice: request.BasePrice, cGST: request.CGST, sGST: request.SGST, purchasePrice: request.PurchasePrice, taxType: request.TaxType, profitMargin: request.ProfitMargin, profitMarginAmount: request.ProfitMarginAmount, salesPrice: request.SalesPrice, imagePath: productImagePath, isActive: request.IsActive);

        // Add Domain Events to be raised after the commit
        product.DomainEvents.Add(EntityCreatedEvent.WithEntity(product));

        await _repository.AddAsync(product, cancellationToken);

        return product.Id;
    }
}