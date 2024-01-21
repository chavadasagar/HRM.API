using HRM.API.Domain.Catalog.Sales.Taxs;

namespace HRM.API.Domain.Catalog.Sales.Expenses;
public class Expense : AuditableEntity, IAggregateRoot
{
    public Guid? ItemId { get; set; }
    public string? PurchaseFrom { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public Guid? PurchaseById { get; set; }
    public decimal? Amount { get; set; }
    public Guid? PaidById { get; set; }
    public Guid? TaxStatusId { get; set; }
    public List<ExpenseAttachments>? ExpenseAttachments { get; set; }
    public virtual Item? Item { get; set; }
    public virtual Employee? PurchaseBy { get; set; }
    public virtual PaymentType? PaidBy { get; set; }
    public virtual TaxStatus? TaxStatus { get; set; }
}
