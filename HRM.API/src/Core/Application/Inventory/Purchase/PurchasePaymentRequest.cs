namespace MasterPOS.API.Application.Inventory;
public class PurchasePaymentRequest
{
    public Guid PaymentTypeId { get; set; } = default!;
    public decimal Amount { get; set; } = default!;
    public string? Note { get; set; }
}