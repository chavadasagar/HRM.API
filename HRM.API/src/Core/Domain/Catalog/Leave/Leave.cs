namespace HRM.API.Domain.Catalog;
public class Leave : AuditableEntity, IAggregateRoot
{
    public Guid? LeaveTypeId { get; set; }
    public Guid? LeaveStatusId { get; set; }
    public Guid? EmployeeId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string? Reason { get; set; }
    public virtual LeaveType? LeaveType { get; set; }
    public virtual LeaveStatus? LeaveStatus { get; set; }
    public virtual Employee? Employee { get; set; }
}
