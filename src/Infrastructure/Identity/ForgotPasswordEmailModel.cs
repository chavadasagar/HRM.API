namespace HRM.API.Infrastructure.Identity;
public class ForgotPasswordEmailModel
{
    public string UserName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Url { get; set; } = default!;
    public string ForgotLink { get; set; }
    public string? CompanyLogoPath { get; set; }
    public string? CompanyName { get; set; }
    public string? DirectorName { get; set; }
    public string? Website { get; set; }
    public string? CompanyEmail { get; set; }
    public string? Phone { get; set; }
    public string? Mobile { get; set; }
    public string? CountryName { get; set; }
    public string? StateName { get; set; }
    public string? City { get; set; }
    public string? Postcode { get; set; }
    public string? Address { get; set; }
}
