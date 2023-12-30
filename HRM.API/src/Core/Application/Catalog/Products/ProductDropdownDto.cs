namespace HRM.API.Application.Catalog.Products;
public class ProductDropdownDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Code { get; set; } = default!;
    public string? SKU { get; set; }
    public string? HSN { get; set; }
    public string? Barcode { get; set; }
    public string? ImagePath { get; set; }

}
