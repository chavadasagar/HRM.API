namespace MasterPOS.API.Domain.Catalog;
public class Unit : AuditableEntity, IAggregateRoot
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public bool IsActive { get; set; }

    public Unit(string name, string? description, bool isActive)
    {
        Name = name;
        Description = description;
        IsActive = isActive;
    }

    public Unit Update(string? name, string? description, bool isActive)
    {
        if (name is not null && Name?.Equals(name) is not true) Name = name;
        if (description is not null && Description?.Equals(description) is not true) Description = description;
        IsActive = isActive;
        return this;
    }
}
