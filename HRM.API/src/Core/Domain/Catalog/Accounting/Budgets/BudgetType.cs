namespace HRM.API.Domain.Catalog.Accounting.Budgets;
public class BudgetType : AuditableEntity, IAggregateRoot
{
    public BudgetType(string? name, string? description)
    {
        Name = name;
        Description = description;
    }

    public string? Name { get; set; } = BudgetTypeEnum.Project.ToString();
    public string? Description { get; set; }
}

public enum BudgetTypeEnum
{
    Project,
    Category
}