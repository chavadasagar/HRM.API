namespace HRM.API.Domain.Catalog;
public class State : AuditableEntity, IAggregateRoot
{
    public State()
    {
    }

    public State(string name, string? latitude, string? longitude)
    {
        Name = name;
        this.Latitude = latitude;
        this.Longitude = longitude;
    }

    public State(string name, string? latitude, string? longitude, List<City>? cities)
    {
        Name = name;
        this.Latitude = latitude;
        this.Longitude = longitude;
        Cities = cities;
    }

    public string Name { get; private set; }
    public string? Latitude { get; private set; }
    public string? Longitude { get; private set; }
    public Guid? CountryId { get; private set; }
    public List<City>? Cities { get; private set; }
}