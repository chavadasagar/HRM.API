namespace HRM.API.Domain.Catalog;
public class Country : AuditableEntity, IAggregateRoot
{
    public Country(string name, string? latitude, string? longitude)
    {
        Name = name;
        this.Latitude = latitude;
        this.Longitude = longitude;
    }

    public Country(Country country)
    {
        this.Name = country.Name;
        this.Latitude = country.Latitude;
        this.Longitude = country.Longitude;
        this.States = country.States;
    }

    public Country(string name, string? latitude, string? longitude, List<State>? states)
    {
        Name = name;
        this.Latitude = latitude;
        this.Longitude = longitude;
        States = states;
    }

    public Country()
    {
    }

    public string Name { get; set; }
    public string? Latitude { get; set; }
    public string? Longitude { get; set; }
    public List<State>? States { get; set; }
}