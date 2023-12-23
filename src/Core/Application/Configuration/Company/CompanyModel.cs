namespace HRM.API.Application.Configuration;
public class CompanyModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? DirectorName { get; set; }
    public string Email { get; set; } = default!;
    public string? Mobile { get; set; }
    public string? Phone { get; set; }
    public string? GSTNumber { get; set; }
    public string? VATNumber { get; set; }
    public string? PANNumber { get; set; }
    public string? Website { get; set; }
    public string? UPIId { get; set; }
    public string? BankDetails { get; set; }
    public string? CountryName { get; set; }
    public string? StateName { get; set; }
    public string? City { get; set; }
    public string? Postcode { get; set; }
    public string? Address { get; set; }
    public string? CompanyLogoPath { get; set; }
}
