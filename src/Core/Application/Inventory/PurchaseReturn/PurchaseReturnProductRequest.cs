using HRM.API.Domain.Inventory;

namespace HRM.API.Application.Inventory;
public class PurchaseReturnProductRequest
{
    public Guid? Id { get; set; }
    public Guid? PurchaseReturnId { get; set; }
    public Guid ProductId { get; set; }
    public Guid StoreId { get; set; }
    public string? ProductName { get; set; }
    public decimal Quantity { get; set; }
    public decimal PurchasePrice { get; set; }
    public decimal? Discount { get; set; }
    public short? DiscountType { get; set; }
    public decimal? TotalDiscount { get; set; }
    public decimal CGST { get; set; }
    public decimal SGST { get; set; }
    public short TaxType { get; set; }
    public decimal TotalTax { get; set; }
    public decimal UnitCostAfterTaxAndDiscount { get; set; }
    public decimal TotalAmount { get; set; }
}
