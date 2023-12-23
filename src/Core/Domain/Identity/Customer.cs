using HRM.API.Domain.Catalog;

namespace HRM.API.Domain.Identity;
public class Customer : AuditableEntity, IAggregateRoot
{
    public string Name { get; private set; }
    public string? Mobile { get; private set; }
    public string? Email { get; private set; }
    public string? Phone { get; private set; }
    public string? GSTNumber { get; private set; }
    public Guid? CountryId { get; private set; }
    public Guid? StateId { get; private set; }
    public string? City { get; private set; }
    public string? Postcode { get; private set; }
    public string? Address { get; private set; }
    public bool IsPrimaryCustomer { get; set; }
    public bool IsActive { get; private set; }
    public virtual Country Country { get; private set; } = default!;
    public virtual States State { get; private set; } = default!;

    public Customer(string name, string? mobile, string? email, string? phone, string? gSTNumber, Guid? countryId, Guid? stateId, string? city, string? postcode, string? address, bool isActive)
    {
        Name = name;
        Mobile = mobile;
        Email = email;
        Phone = phone;
        GSTNumber = gSTNumber;
        CountryId = null;
        if (countryId.HasValue && countryId.Value != Guid.Empty && !CountryId.Equals(countryId.Value)) CountryId = countryId.Value;
        StateId = null;
        if (stateId.HasValue && stateId.Value != Guid.Empty && !StateId.Equals(stateId.Value)) StateId = stateId.Value;
        City = city;
        Postcode = postcode;
        Address = address;
        IsActive = isActive;
        IsPrimaryCustomer = false;
    }

    public Customer Update(string? name, string? mobile, string? email, string? phone, string? gSTNumber,     Guid? countryId, Guid? stateId, string? city, string? postcode, string? address, bool isActive)
    {
        if (name is not null && Name?.Equals(name) is not true) Name = name;
        if (mobile is not null && Mobile?.Equals(mobile) is not true) Mobile = mobile;
        if (email is not null && Email?.Equals(email) is not true) Email = email;
        if (phone is not null && Phone?.Equals(phone) is not true) Phone = phone;
        if (gSTNumber is not null && GSTNumber?.Equals(gSTNumber) is not true) GSTNumber = gSTNumber;
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

    public Customer UpdateStatus(bool isActive)
    {
        IsActive = isActive;
        return this;
    }
}
