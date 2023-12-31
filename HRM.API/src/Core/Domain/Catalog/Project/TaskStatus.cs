namespace HRM.API.Domain.Catalog;
public class TaskStatus : AuditableEntity, IAggregateRoot
{
    public TaskStatus(string? name, string? description)
    {
        Name = name;
        Description = description;
    }

    public string? Name { get; set; } = "Pending";
    public string? Description { get; set; }
}
