namespace HRM.API.Domain.Catalog;
public class TaskPriority : AuditableEntity, IAggregateRoot
{
    public TaskPriority(string? name, string? description)
    {
        Name = name;
        Description = description;
    }

    public string? Name { get; set; }
    public string? Description { get; set; }
}
