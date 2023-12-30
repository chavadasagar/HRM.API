namespace HRM.API.Domain.Catalog;
public class Overtime : AuditableEntity, IAggregateRoot
{
    public Overtime(DefaultIdType? employeeId, DateTime? date, decimal? hours, string? description)
    {
        EmployeeId = employeeId;
        Date = date;
        Hours = hours;
        Description = description;
    }

    public DefaultIdType? EmployeeId { get; set; }
    public DateTime? Date { get; set; }
    public decimal? Hours { get; set; }
    public string? Description { get; set; }
    public DefaultIdType? OvertimeStatusId { get; set; }
    public virtual Employee? Employee { get; set; }
    public virtual OvertimeStatus? OvertimeStatus { get; set; }
}
