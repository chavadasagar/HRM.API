using HRM.API.Domain.Inventory;

namespace HRM.API.Domain.Catalog;
public class Supplier : AuditableEntity, IAggregateRoot
{
    public string Name { get; private set; }
    public string Mobile { get; private set; }
    public string? Email { get; private set; }
    public string? Phone { get; private set; }
    public string? GSTNumber { get; private set; }
    public DateTime? BirthDate { get; set; }
    public DateTime? AnniversaryDate { get; set; }
    public Guid? CountryId { get; private set; }
    public Guid? StateId { get; private set; }
    public string? City { get; private set; }
    public string? Postcode { get; private set; }
    public string? Address { get; private set; }
    public bool IsActive { get; private set; }
    public virtual Country Country { get; private set; } = default!;
    public virtual State State { get; private set; } = default!;
    public List<Purchase>? PurchaseList { get; set; }
    public List<PurchaseReturn>? PurchaseReturnList { get; set; }

    public Supplier(string name, string mobile, string? email, string? phone, string? gSTNumber, DateTime? birthDate, DateTime? anniversaryDate, Guid? countryId, Guid? stateId, string? city, string? postcode, string? address, bool isActive)
    {
        Name = name;
        Mobile = mobile;
        Email = email;
        Phone = phone;
        GSTNumber = gSTNumber;
        if (birthDate is not null) BirthDate = birthDate;
        if (anniversaryDate is not null) AnniversaryDate = anniversaryDate;
        CountryId = null;
        if (countryId.HasValue && countryId.Value != Guid.Empty && !CountryId.Equals(countryId.Value)) CountryId = countryId.Value;
        StateId = null;
        if (stateId.HasValue && stateId.Value != Guid.Empty && !StateId.Equals(stateId.Value)) StateId = stateId.Value;
        City = city;
        Postcode = postcode;
        Address = address;
        IsActive = isActive;
    }

    public Supplier Update(string? name, string? mobile, string? email, string? phone, string? gSTNumber, DateTime? birthDate, DateTime? anniversaryDate, Guid? countryId, Guid? stateId, string? city, string? postcode, string? address, bool isActive)
    {
        if (name is not null && Name?.Equals(name) is not true) Name = name;
        if (mobile is not null && Mobile?.Equals(mobile) is not true) Mobile = mobile;
        if (email is not null && Email?.Equals(email) is not true) Email = email;
        if (phone is not null && Phone?.Equals(phone) is not true) Phone = phone;
        if (gSTNumber is not null && GSTNumber?.Equals(gSTNumber) is not true) GSTNumber = gSTNumber;
        BirthDate = null;
        if (birthDate is not null) BirthDate = birthDate;
        AnniversaryDate = null;
        if (anniversaryDate is not null) AnniversaryDate = anniversaryDate;
        CountryId = null;
        if (countryId.HasValue && countryId.Value != Guid.Empty && !CountryId.Equals(countryId.Value)) CountryId = countryId.Value;
        StateId = null;
        if (stateId.HasValue && stateId.Value != Guid.Empty && !StateId.Equals(stateId.Value)) StateId = stateId.Value;
        if (city is not null && City?.Equals(city) is not true) City = city;
        Postcode = postcode;
        if (address is not null && Address?.Equals(address) is not true) Address = address;
        IsActive = isActive;
        return this;
    }

    public Supplier UpdateStatus(bool isActive)
    {
        IsActive = isActive;
        return this;
    }
}
