using HRM.API.Domain.Catalog;

namespace HRM.API.Domain.Configuration;
public class Company : AuditableEntity, IAggregateRoot
{
    public string Name { get; private set; }
    public string? DirectorName { get; private set; }
    public string Email { get; private set; }
    public string? Mobile { get; private set; }
    public string? Phone { get; private set; }
    public string? GSTNumber { get; private set; }
    public string? VATNumber { get; private set; }
    public string? PANNumber { get; private set; }
    public string? Website { get; private set; }
    public string? UPIId { get; private set; }
    public string? BankDetails { get; private set; }
    public Guid? CountryId { get; private set; }
    public Guid? StateId { get; private set; }
    public Guid? CityId { get; private set; }
    public string? Postcode { get; private set; }
    public string? Address { get; private set; }
    public string? CompanyLogoPath { get; private set; }
    public bool IsActive { get; private set; }
    public virtual Country Country { get; private set; } = default!;
    public virtual State State { get; private set; } = default!;
    public virtual City City { get; private set; } = default!;

    public Company(string name, string email, string? directorName, string? mobile, string? phone, string? gSTNumber, string? vATNumber, string? pANNumber, string? website, string? uPIId, string? bankDetails, Guid? countryId, Guid? stateId, Guid? cityId, string? postcode, string? address, string? companyLogoPath, bool isActive)
    {
        Name = name;
        Email = email;
        DirectorName = directorName;
        Mobile = mobile;
        Phone = phone;
        GSTNumber = gSTNumber;
        VATNumber = vATNumber;
        PANNumber = pANNumber;
        Website = website;
        UPIId = uPIId;
        BankDetails = bankDetails;
        CountryId = null;
        if (countryId.HasValue && countryId.Value != Guid.Empty && !CountryId.Equals(countryId.Value)) CountryId = countryId.Value;
        StateId = null;
        if (stateId.HasValue && stateId.Value != Guid.Empty && !StateId.Equals(stateId.Value)) StateId = stateId.Value;
        CityId = cityId;
        Postcode = postcode;
        Address = address;
        CompanyLogoPath = companyLogoPath;
        IsActive = isActive;
    }

    public Company Update(string? name, string? email, string? directorName, string? mobile, string? phone, string? gSTNumber, string? vATNumber, string? pANNumber, string? website, string? uPIId, string? bankDetails, Guid? countryId, Guid? stateId, Guid? cityId, string? postcode, string? address, string? companyLogoPath)
    {
        if (name is not null && Name?.Equals(name) is not true) Name = name;
        if (email is not null && DirectorName?.Equals(email) is not true) Email = email;
        if (directorName is not null && DirectorName?.Equals(directorName) is not true) DirectorName = directorName;
        if (mobile is not null && Mobile?.Equals(mobile) is not true) Mobile = mobile;
        if (phone is not null && Phone?.Equals(phone) is not true) Phone = phone;
        if (gSTNumber is not null && GSTNumber?.Equals(gSTNumber) is not true) GSTNumber = gSTNumber;
        if (vATNumber is not null && VATNumber?.Equals(vATNumber) is not true) VATNumber = vATNumber;
        if (pANNumber is not null && PANNumber?.Equals(pANNumber) is not true) PANNumber = pANNumber;
        if (website is not null && Website?.Equals(website) is not true) Website = website;
        if (uPIId is not null && UPIId?.Equals(uPIId) is not true) UPIId = uPIId;
        if (bankDetails is not null && BankDetails?.Equals(bankDetails) is not true) BankDetails = bankDetails;
        CountryId = null;
        if (countryId.HasValue && countryId.Value != Guid.Empty && !CountryId.Equals(countryId.Value)) CountryId = countryId.Value;
        StateId = null;
        if (stateId.HasValue && stateId.Value != Guid.Empty && !StateId.Equals(stateId.Value)) StateId = stateId.Value;
        if (cityId is not null && CityId?.Equals(cityId) is not true) CityId = cityId;
        Postcode = postcode;
        if (address is not null && Address?.Equals(address) is not true) Address = address;
        if (companyLogoPath is not null && CompanyLogoPath?.Equals(companyLogoPath) is not true) CompanyLogoPath = companyLogoPath;
        return this;
    }

    public Company UpdateStatus(bool isActive)
    {
        IsActive = isActive;
        return this;
    }

    public Company ClearImagePath()
    {
        CompanyLogoPath = string.Empty;
        return this;
    }
}
