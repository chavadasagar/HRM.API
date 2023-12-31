namespace HRM.API.Domain.Catalog;
public class ProjectTask : AuditableEntity, IAggregateRoot
{
    public Guid? ProjectId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public Guid? ProjectTaskId { get; set; }
    public Guid? EmployeeId { get; set; }
    public virtual Project? Project { get; set; }
    public virtual TaskStatus? TaskStatus { get; set; }
    public virtual Employee? Employee { get; set; }
}