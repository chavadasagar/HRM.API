namespace HRM.API.Domain.Catalog;
public class ProjectTeamLeaders : AuditableEntity, IAggregateRoot
{
    public ProjectTeamLeaders(DefaultIdType? employeeId, DefaultIdType? projectId)
    {
        EmployeeId = employeeId;
        ProjectId = projectId;
    }

    public Guid? EmployeeId { get; set; }
    public Guid? ProjectId { get; set; }
    public virtual Employee? Employee { get; set; }
    public virtual Project? Project { get; set; }
}
