using MasterPOS.API.Domain.Inventory;

namespace MasterPOS.API.Application.Inventory;
public class PurchaseReturnDto : IDto
{
    public Guid Id { get; set; }
    public string? PurchaseReturnInvoiceId { get; set; }
    public string? ReferenceNo { get; set; }
    public string? PurchaseInvoiceId { get; set; }
    public Guid StoreId { get; set; }
    public Guid SupplierId { get; set; }
    public DateTime PurchaseReturnDate { get; set; }
    public short PurchaseReturnStatus { get; set; }
    public decimal TotalQuantity { get; set; }
    public decimal CGST { get; set; }
    public decimal SGST { get; set; }
    public decimal TotalTax { get; set; }
    public decimal? OtherCharge { get; set; }
    public decimal? Discount { get; set; }
    public short? DiscountType { get; set; }
    public decimal? TotalDiscount { get; set; }
    public decimal SubtotalAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public string? Note { get; set; }
    public string? StoreName { get; set; }
    public string? SupplierName { get; set; }
    public List<PurchaseReturnProductDto>? PurchaseReturnProducts { get; set; }
    public List<PurchaseReturnPaymentDto>? PurchaseReturnPayments { get; set; }
}
