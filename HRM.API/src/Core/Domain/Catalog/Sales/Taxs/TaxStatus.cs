namespace HRM.API.Domain.Catalog.Sales.Taxs;
public class TaxStatus : AuditableEntity, IAggregateRoot
{
    public TaxStatus(string? name, string? description)
    {
        Name = name;
        Description = description;
    }

    public string? Name { get; set; } = "Approved";
    public string? Description { get; set; }
}
