namespace HRM.API.Domain.Catalog;
public class RateType : AuditableEntity, IAggregateRoot
{
    public RateType(string? name, string? description)
    {
        Name = name;
        Description = description;
    }

    public string? Name { get; set; } = "Hourly";
    public string? Description { get; set; }
}
