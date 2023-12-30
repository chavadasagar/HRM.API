namespace HRM.API.Domain.Catalog;
public class LeaveStatus : AuditableEntity, IAggregateRoot
{
    public string? Name { get; set; }
    public bool? IsActive { get; set; }
}
