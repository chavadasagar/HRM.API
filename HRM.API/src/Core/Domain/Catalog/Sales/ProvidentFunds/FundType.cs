namespace HRM.API.Domain.Catalog.Sales.ProvidentFunds;
public class FundType : AuditableEntity, IAggregateRoot
{
    public FundType(string? name, string? description)
    {
        Name = name;
        Description = description;
    }

    public string? Name { get; set; } = "Basic salary";
    public string? Description { get; set; }
}
