using HRM.API.Domain.Catalog;

namespace HRM.API.Domain.Inventory;
public class PurchasePayment : AuditableEntity, IAggregateRoot
{
    public Guid PurchaseId { get; private set; } = default!;
    public Guid SupplierId { get; private set; } = default!;
    public Guid PaymentTypeId { get; private set; } = default!;
    public decimal Amount { get; private set; } = default!;
    public DateTime PaymentDate { get; private set; }
    public string? Note { get; private set; }
    public virtual Purchase Purchase { get; private set; } = default!;
    public virtual PaymentType PaymentType { get; private set; } = default!;

    public PurchasePayment(Guid purchaseId, Guid supplierId, Guid paymentTypeId, decimal amount, DateTime paymentDate, string note)
    {
        PurchaseId = purchaseId;
        SupplierId = supplierId;
        PaymentTypeId = paymentTypeId;
        Amount = amount;
        PaymentDate = paymentDate;
        Note = note;
    }
}
