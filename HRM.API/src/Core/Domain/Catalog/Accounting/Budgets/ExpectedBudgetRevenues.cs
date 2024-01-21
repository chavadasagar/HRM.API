namespace HRM.API.Domain.Catalog.Accounting.Budgets;
public class ExpectedBudgetRevenues : AuditableEntity, IAggregateRoot
{
    public DefaultIdType? BudgetId { get; set; }
    public string? Title { get; set; }
    public decimal? Amount { get; set; }
    public virtual Budget? Budget { get; set; }
}
