namespace HRM.API.Domain.Catalog;
public class LeaveType : AuditableEntity, IAggregateRoot
{
    public string? Name { get; set; }
    public bool? IsActive { get; set; }
}
