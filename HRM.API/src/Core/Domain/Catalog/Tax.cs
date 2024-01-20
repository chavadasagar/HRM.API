namespace HRM.API.Domain.Catalog;
public class Tax : AuditableEntity, IAggregateRoot
{
    public Tax(string? name, string? description)
    {
        Name = name;
        Description = description;
    }

    public string? Name { get; set; } = "GST";
    public string? Description { get; set; }
}
