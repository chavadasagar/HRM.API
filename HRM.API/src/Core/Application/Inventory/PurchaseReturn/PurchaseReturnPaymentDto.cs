namespace MasterPOS.API.Application.Inventory;
public class PurchaseReturnPaymentDto : IDto
{
    public Guid Id { get; set; } = default!;
    public Guid PurchaseId { get; set; } = default!;
    public Guid SupplierId { get; set; } = default!;
    public Guid PaymentTypeId { get; set; } = default!;
    public string? PaymentTypeName { get; set; }
    public decimal Amount { get; set; } = default!;
    public DateTime PaymentDate { get; set; }
    public string? Note { get; set; }

}
