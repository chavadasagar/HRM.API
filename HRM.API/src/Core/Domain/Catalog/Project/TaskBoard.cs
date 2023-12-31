namespace HRM.API.Domain.Catalog;
public class TaskBoard : AuditableEntity, IAggregateRoot
{
    public string? Name { get; set; }
    public Guid? TaskPriorityId { get; set; }
    public DateTime? DueDate { get; set; }
    public virtual List<TaskBoardFollowers>? TaskBoardFollowers { get; set; }
    public virtual TaskPriority? TaskPriority { get; set; }
}
