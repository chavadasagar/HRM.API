namespace HRM.API.Domain.Catalog.Sales.Taxs;
public class Tax : AuditableEntity, IAggregateRoot
{
    public Tax(string? name, string? description, Guid? statusId)
    {
        Name = name;
        Description = description;
        StatusId = statusId;
    }

    public string? Name { get; set; } = "GST";
    public string? Description { get; set; }
    public Guid? StatusId { get; set; }
    public virtual TaxStatus? Status { get; set; }
}
