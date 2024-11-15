namespace HRM.API.Application.Configuration;
public class CompanyDto : IDto
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = default!;
    public string? DirectorName { get; private set; }
    public string Email { get; private set; } = default!;
    public string? Mobile { get; private set; }
    public string? Phone { get; private set; }
    public string? GSTNumber { get; private set; }
    public string? VATNumber { get; private set; }
    public string? PANNumber { get; private set; }
    public string? Website { get; private set; }
    public string? UPIId { get; private set; }
    public string? BankDetails { get; private set; }
    public string? CountryName { get; private set; }
    public Guid? CountryId { get; private set; }
    public string? StateName { get; private set; }
    public Guid? StateId { get; private set; }
    public string? City { get; private set; }
    public string? Postcode { get; private set; }
    public string? Address { get; private set; }
    public string? CompanyLogoPath { get; private set; }
    public bool IsActive { get; private set; }

}
