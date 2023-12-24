using HRM.API.Domain.Configuration;

namespace HRM.API.Domain.Catalog;
public class Employee : AuditableEntity, IAggregateRoot
{
    public Employee(string? firstname, string? lastname, string? username, string? email, string? password, string? employeeId, DateTime? joiningDate, string? phone, DefaultIdType? companyId, DefaultIdType? departmentId, DefaultIdType? designationId)
    {
        Firstname = firstname;
        Lastname = lastname;
        Username = username;
        Email = email;
        Password = password;
        EmployeeId = employeeId;
        JoiningDate = joiningDate;
        Phone = phone;
        CompanyId = companyId;
        DepartmentId = departmentId;
        DesignationId = designationId;
    }

    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? EmployeeId { get; set; }
    public DateTime? JoiningDate { get; set; }
    public string? Phone { get; set; }
    public Guid? CompanyId { get; set; }
    public Guid? DepartmentId { get; set; }
    public Guid? DesignationId { get; set; }
    public bool IsActive { get; private set; }
    public virtual Company? Company { get; set; }
    public virtual Department? Department { get; set; }
    public virtual Designation? Designation { get; set; }
}
