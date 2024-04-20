namespace HRM.API.Domain.Catalog;
public class Attendance : AuditableEntity, IAggregateRoot
{
    public Attendance()
    {
            
    }
    public Attendance(DefaultIdType? employeeId, DateTime? date, DateTime? punchIn, DateTime? punchOut, string? production, string? @break, string? overTime)
    {
        EmployeeId = employeeId;
        Date = date;
        PunchIn = punchIn;
        PunchOut = punchOut;
        Production = production;
        Break = @break;
        OverTime = overTime;
    }

    public DefaultIdType? EmployeeId { get; set; }
    public DateTime? Date { get; set; }
    public DateTime? PunchIn { get; set; }
    public DateTime? PunchOut { get; set; }
    public string? Production { get; set; }
    public string? Break { get; set; }
    public string? OverTime { get; set; }
    public virtual Employee? Employee { get; set; }
}
