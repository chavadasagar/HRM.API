namespace HRM.API.Application.Identity.Customers;

public class CustomerDto : IDto
{
    public Guid Id { get; set; }
    public string Name { get; private set; } = default!;
    public string? Mobile { get; private set; }
    public string? Email { get; private set; }
    public string? Phone { get; private set; }
    public string? GSTNumber { get; private set; }
    public string? CountryName { get; private set; }
    public Guid? CountryId { get; private set; }
    public string? StateName { get; private set; }
    public Guid? StateId { get; private set; }
    public string? City { get; private set; }
    public string? Postcode { get; private set; }
    public string? Address { get; private set; }
    public bool IsPrimaryCustomer { get; set; }
    public bool IsActive { get; private set; }
}