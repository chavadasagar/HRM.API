namespace HRM.API.Domain.Catalog;
public class TaskBoardFollowers : AuditableEntity, IAggregateRoot
{
    public Guid? TaskBoardId { get; set; }
    public Guid? EmployeeId { get; set; }
    public virtual Employee? Employee { get; set; }
    public virtual TaskBoard? TaskBoard { get; set; }
}
