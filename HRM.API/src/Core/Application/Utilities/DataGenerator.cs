using Bogus;
using HRM.API.Domain.Enum;

namespace HRM.API.Application.Utilities;
public class DataGenerator
{

    public const int NumberOfEmployees = 5;
    public const int NumberOfAttendancePerEmployee = 2;

    public static readonly List<Employee> Employees = new();
    public static readonly List<Attendance> Attendance = new();
    public static Faker<Employee> GetEmployeeGenerator()
    {
        return new Faker<Employee>()
            .RuleFor(e => e.Id, _ => Guid.NewGuid())
            .RuleFor(e => e.Firstname, f => f.Name.FirstName())
            .RuleFor(e => e.Lastname, f => f.Name.LastName())
            .RuleFor(e => e.Email, (f, e) => f.Internet.Email(e.Firstname, e.Lastname))
            .RuleFor(e => e.AboutMe, f => f.Lorem.Paragraph(1))
            .RuleFor(e => e.Personality, f => f.PickRandom<Personality>())
            .RuleFor(e => e.Attendance, (_, e) =>
            {
                return GetAttendanceData(e.Id);
            });
    }
    public static Faker<Attendance> GetAttendanceGenerator(Guid? employeeId)
    {
        return new Faker<Attendance>()
            .RuleFor(a => a.EmployeeId, _ => employeeId)
            .RuleFor(a => a.Date, f => f.Date.Between(DateTime.Now.AddYears(-1), DateTime.Now))
            .RuleFor(a => a.PunchIn, f => f.Date.Between(DateTime.Now.AddMinutes(-10), DateTime.Now))
            .RuleFor(a => a.PunchOut, f => f.Date.Between(DateTime.Now.AddMinutes(-10), DateTime.Now))
            .RuleFor(a => a.Production, f => Math.Round(f.Random.Double(1, 12), 1) + " hours") // Random decimal number of hours, rounded to one decimal place
            .RuleFor(a => a.Break, f => Math.Round(f.Random.Double(1, 4), 1) + " hours") // Random decimal number of hours for break
            .RuleFor(a => a.OverTime, f => Math.Round(f.Random.Double(0, 8), 1) + " hours"); // Random decimal number of hours for overtime
    }
    private static List<Attendance> GetAttendanceData(Guid employeeId)
    {
        var vehicleGenerator = GetAttendanceGenerator(employeeId);
        var generatedAttendance = vehicleGenerator.Generate(NumberOfAttendancePerEmployee);
        Attendance.AddRange(generatedAttendance);
        return generatedAttendance;
    }
}
