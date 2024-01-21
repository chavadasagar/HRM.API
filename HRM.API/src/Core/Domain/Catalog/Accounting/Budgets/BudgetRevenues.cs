﻿namespace HRM.API.Domain.Catalog.Accounting.Budgets;
public class BudgetRevenues : AuditableEntity, IAggregateRoot
{
    public decimal? Amount { get; set; }
    public string? Notes { get; set; }
    public DateTime? RevenuesDate { get; set; }
    public Guid? CategoryId { get; set; }
    public Guid? SubCategoryId { get; set; }
    public string? FilePath { get; set; }
    public virtual Category? Category { get; set; }
    public virtual SubCategory? SubCategory { get; set; }
}
