using HRM.API.Domain.Catalog;

namespace HRM.API.Domain.Inventory;
public class PurchaseReturnPayment : AuditableEntity, IAggregateRoot
{
    public Guid PurchaseReturnId { get; private set; } = default!;
    public Guid SupplierId { get; private set; } = default!;
    public Guid PaymentTypeId { get; private set; } = default!;
    public decimal Amount { get; private set; } = default!;
    public DateTime PaymentDate { get; private set; }
    public string? Note { get; private set; }
    public virtual PurchaseReturn Purchase { get; private set; } = default!;
    public virtual PaymentType PaymentType { get; private set; } = default!;

    public PurchaseReturnPayment(Guid purchaseReturnId, Guid supplierId, Guid paymentTypeId, decimal amount, DateTime paymentDate, string note)
    {
        PurchaseReturnId = purchaseReturnId;
        SupplierId = supplierId;
        PaymentTypeId = paymentTypeId;
        Amount = amount;
        PaymentDate = paymentDate;
        Note = note;
    }
}
