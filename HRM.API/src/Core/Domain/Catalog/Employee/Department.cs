namespace HRM.API.Domain.Catalog;
public class Department : AuditableEntity, IAggregateRoot
{
    public Department(string? name, bool isActive)
    {
        Name = name;
        IsActive = isActive;
    }

    public string? Name { get; set; }
    public bool IsActive { get; private set; }
}
