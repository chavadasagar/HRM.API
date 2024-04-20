using HRM.API.Application.Catalog.Attendance;

namespace HRM.API.Application.Catalog.Employees;
public class EmployeeDto : IDto
{
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? EmployeeId { get; set; }
    public DateTime? JoiningDate { get; set; }
    public string? Phone { get; set; }
    public List<AttendanceDto>? Attendance { get; set; }
}
