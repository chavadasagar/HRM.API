namespace HRM.API.Domain.Catalog;
public class ProjectTaskBoard : AuditableEntity, IAggregateRoot
{
    public Guid? ProjectId { get; set; }
    public List<TaskBoard>? TaskBoard { get; set; }
    public virtual Project? Project { get; set; }
}
