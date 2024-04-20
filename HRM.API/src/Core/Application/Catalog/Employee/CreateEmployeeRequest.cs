using Mapster;
namespace HRM.API.Application.Catalog.Employees;
public class CreateEmployeeRequest : IRequest<Guid>
{
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? ConfirmPassword { get; set; }
    public string? EmployeeId { get; set; }
    public DateTime? JoiningDate { get; set; }
    public string? Phone { get; set; }
    public DefaultIdType? CompanyId { get; set; }
    public DefaultIdType? DepartmentId { get; set; }
    public DefaultIdType? DesignationId { get; set; }
}

public class CreateEmployeeRequestValidator : CustomValidator<CreateEmployeeRequest>
{
    public CreateEmployeeRequestValidator()
    {
        RuleFor(x => x.Firstname).NotEmpty().WithMessage("Firstname is required.");
        RuleFor(x => x.Lastname).NotEmpty().WithMessage("Lastname is required.");
        RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required.");
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.").EmailAddress().WithMessage("Invalid email format.");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.").MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
        RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("Passwords do not match.");
        RuleFor(x => x.EmployeeId).NotEmpty().WithMessage("Employee ID is required.");
        RuleFor(x => x.JoiningDate).NotNull().WithMessage("Joining date is required.").LessThan(DateTime.Now).WithMessage("Joining date cannot be in the future.");
        RuleFor(x => x.Phone).NotEmpty().WithMessage("Phone number is required.");
        RuleFor(x => x.CompanyId).NotNull().WithMessage("Company ID is required.");
        RuleFor(x => x.DepartmentId).NotNull().WithMessage("Department ID is required.");
        RuleFor(x => x.DesignationId).NotNull().WithMessage("Designation ID is required.");
    }
}

public class CreateEmployeeRequestHandler : IRequestHandler<CreateEmployeeRequest, Guid>
{
    private readonly IRepository<Employee> _employee;

    public async Task<DefaultIdType> Handle(CreateEmployeeRequest request, CancellationToken cancellationToken)
    {
        var employee = await _employee.AddAsync(request.Adapt<Employee>(), cancellationToken);

        return employee.Id;
    }
}