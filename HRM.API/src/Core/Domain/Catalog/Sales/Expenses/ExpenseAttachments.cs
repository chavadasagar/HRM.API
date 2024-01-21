namespace HRM.API.Domain.Catalog.Sales.Expenses;
public class ExpenseAttachments : AuditableEntity, IAggregateRoot
{
    public Guid? ExpenseId { get; set; }
    public string? FilePath { get; set; }
    public virtual Expense? Expense { get; set; }
}
