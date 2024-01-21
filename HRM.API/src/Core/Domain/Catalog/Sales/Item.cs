namespace HRM.API.Domain.Catalog.Sales;
public class Item : AuditableEntity, IAggregateRoot
{
    public Item(string? name, string? description)
    {
        Name = name;
        Description = description;
    }

    public string? Name { get; set; } = "Laptop";
    public string? Description { get; set; }
}