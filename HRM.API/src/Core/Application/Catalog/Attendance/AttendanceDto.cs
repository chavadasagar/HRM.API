namespace HRM.API.Application.Catalog.Attendance;
public class AttendanceDto : IDto
{
    public DefaultIdType? EmployeeId { get; set; }
    public DateTime? Date { get; set; }
    public DateTime? PunchIn { get; set; }
    public DateTime? PunchOut { get; set; }
    public string? Production { get; set; }
    public string? Break { get; set; }
    public string? OverTime { get; set; }
}
