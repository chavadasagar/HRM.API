namespace HRM.API.Domain.Catalog;
public class Designation : AuditableEntity, IAggregateRoot
{
    public string? Name { get; set; }
    public DefaultIdType? DepartmentId { get; set; }
    public bool IsActive { get; private set; }
    public virtual Department? Department { get; set; }
}
