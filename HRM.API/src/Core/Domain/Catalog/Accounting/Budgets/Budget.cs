namespace HRM.API.Domain.Catalog.Accounting.Budgets;
public class Budget : AuditableEntity, IAggregateRoot
{
    public string? BudgetTitle { get; set; }
    public DefaultIdType? BudgetTypeId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public List<ExpectedBudgetRevenues>? Revenues { get; set; }
    public decimal? OverallRevenues { get; set; }
    public List<ExpectedBudgetExpenses>? Expenses { get; set; }
    public decimal? OverallExpenses { get; set; }
    public decimal? ExpectedProfit { get; set; }
    public decimal? TaxAmount { get; set; }
    public decimal? BudgetAmount { get; set; }
    public virtual BudgetType? BudgetType { get; set; }
}
