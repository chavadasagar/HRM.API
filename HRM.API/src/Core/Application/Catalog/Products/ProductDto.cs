namespace HRM.API.Application.Catalog.Products;

public class ProductDto : IDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Code { get; set; } = default!;
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
    public string? ImagePath { get; set; }
    public Guid BrandId { get; set; }
    public string BrandName { get; set; } = default!;
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = default!;
    public Guid UnitId { get; set; }
    public string UnitName { get; set; } = default!;
    public bool IsActive { get; set; }
    public List<ProductByPurchaseProductDto>? PurchaseProducts { get; set; }
    public List<ProductByPurchaseReturnProductDto>? PurchaseReturnProducts { get; set; }
}