namespace HRM.API.Domain.Catalog;

public class Store : AuditableEntity, IAggregateRoot
{
    public string Code { get; private set; }
    public string Name { get; private set; }
    public string? Mobile { get; private set; }
    public string? Email { get; private set; }
    public string? Phone { get; private set; }
    public string? GSTNumber { get; private set; }
    public string? PANNumber { get; private set; }
    public string? BankDetails { get; private set; }
    public Guid? CountryId { get; private set; }
    public Guid? StateId { get; private set; }
    public string? City { get; private set; }
    public string? Postcode { get; private set; }
    public string? Address { get; private set; }
    public string? StoreLogoPath { get; private set; }
    public bool IsDefault { get; private set; }
    public bool IsPrimaryStore { get; set; }
    public bool IsActive { get; private set; }
    public virtual Country Country { get; private set; } = default!;
    public virtual State State { get; private set; } = default!;

    public Store(string code, string name, string? mobile, string? email, string? phone, string? gSTNumber, string? pANNumber, string? bankDetails, Guid? countryId, Guid? stateId, string? city, string? postcode, string? address, string? storeLogoPath, bool isDefault, bool isActive)
    {
        Code = code;
        Name = name;
        Mobile = mobile;
        Email = email;
        Phone = phone;
        GSTNumber = gSTNumber;
        PANNumber = pANNumber;
        BankDetails = bankDetails;
        CountryId = null;
        if (countryId.HasValue && countryId.Value != Guid.Empty && !CountryId.Equals(countryId.Value)) CountryId = countryId.Value;
        StateId = null;
        if (stateId.HasValue && stateId.Value != Guid.Empty && !StateId.Equals(stateId.Value)) StateId = stateId.Value;
        City = city;
        Postcode = postcode;
        Address = address;
        StoreLogoPath = storeLogoPath;
        IsDefault = isDefault;
        IsActive = isActive;
        IsPrimaryStore = false;
    }

    public Store Update(string? code, string? name, string? mobile, string? email, string? phone, string? gSTNumber, string? pANNumber, string? bankDetails, Guid? countryId, Guid? stateId, string? city, string? postcode, string? address, string? storeLogoPath, bool isDefault, bool isActive)
    {
        if (code is not null && Code?.Equals(code) is not true) Code = code;
        if (name is not null && Name?.Equals(name) is not true) Name = name;
        if (mobile is not null && Mobile?.Equals(mobile) is not true) Mobile = mobile;
        if (email is not null && Email?.Equals(email) is not true) Email = email;
        if (phone is not null && Phone?.Equals(phone) is not true) Phone = phone;
        if (gSTNumber is not null && GSTNumber?.Equals(gSTNumber) is not true) GSTNumber = gSTNumber;
        if (pANNumber is not null && PANNumber?.Equals(pANNumber) is not true) PANNumber = pANNumber;
        if (bankDetails is not null && BankDetails?.Equals(bankDetails) is not true) BankDetails = bankDetails;
        CountryId = null;
        if (countryId.HasValue && countryId.Value != Guid.Empty && !CountryId.Equals(countryId.Value)) CountryId = countryId.Value;
        StateId = null;
        if (stateId.HasValue && stateId.Value != Guid.Empty && !StateId.Equals(stateId.Value)) StateId = stateId.Value;
        if (city is not null && City?.Equals(city) is not true) City = city;
        Postcode = postcode;
        if (address is not null && Address?.Equals(address) is not true) Address = address;
        if (storeLogoPath is not null && StoreLogoPath?.Equals(storeLogoPath) is not true) StoreLogoPath = storeLogoPath;
        IsDefault = isDefault;
        IsActive = isActive;
        return this;
    }

    public Store UpdateStatus(bool isActive)
    {
        IsActive = isActive;
        return this;
    }

    public Store ClearImagePath()
    {
        StoreLogoPath = string.Empty;
        return this;
    }
}